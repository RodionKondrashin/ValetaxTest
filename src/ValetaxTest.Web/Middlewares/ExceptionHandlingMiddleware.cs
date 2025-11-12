using System.Text.Json;
using ValetaxTest.Domain.ExceptionJournals;
using ValetaxTest.Domain.Exceptions;
using ValetaxTest.Infrastructure.Postgres;

namespace ValetaxTest.Web.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, dbContext);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, ApplicationDbContext dbContext)
    {
        var eventId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        var queryParams = context.Request.Query.Any()
            ? JsonSerializer.Serialize(context.Request.Query)
            : null;

        string? bodyParams = null;
        if (context.Request.ContentLength > 0)
        {
            context.Request.EnableBuffering();
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body);
            bodyParams = await reader.ReadToEndAsync();
        }
        
        var journal = new ExceptionJournal
        {
            EventId = eventId,
            CreatedAt = DateTime.UtcNow,
            QueryParameters = queryParams,
            BodyParameters = bodyParams,
            StackTrace = exception.StackTrace ?? string.Empty,
            Text = exception.Message
        };
        
        dbContext.ExceptionJournals.Add(journal);
        await dbContext.SaveChangesAsync();
        
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        object response;
        if (exception is SecureException secureEx)
        {
            response = new
            {
                type = secureEx.Type,
                id = secureEx.Id,
                data = new { message = secureEx.Message }
            };
        }
        else
        {
            response = new
            {
                type = "Exception",
                id = eventId,
                data = new { message = $"Internal server error. Event ID = {eventId}" }
            };
            
            _logger.LogError(exception, "Unhandled exception occurred. Event ID: {EventId}", eventId);
        }
        
        await context.Response.WriteAsJsonAsync(response);
        
    }
}