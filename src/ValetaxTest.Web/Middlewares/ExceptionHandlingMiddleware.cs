using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ValetaxTest.Domain.ExceptionJournals;
using ValetaxTest.Domain.Exceptions;
using ValetaxTest.Infrastructure.Postgres;

namespace ValetaxTest.Web.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(
        HttpContext context,
        IServiceScopeFactory serviceScopeFactory)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception caught by middleware");
            await HandleExceptionAsync(context, ex, serviceScopeFactory);
        }
    }
    
    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        IServiceScopeFactory serviceScopeFactory)
    {
        var eventId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        var queryParams = context.Request.Query.Any()
            ? JsonSerializer.Serialize(context.Request.Query.ToDictionary(
                kv => kv.Key,
                kv => kv.Value.ToString()))
            : null;
        
        string? bodyParams = null;
        if (context.Request.ContentLength > 0)
        {
            try
            {
                context.Request.EnableBuffering();
                context.Request.Body.Position = 0;
                using var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true);
                bodyParams = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read request body");
                bodyParams = "[Unable to read body]";
            }
        }
        
        var textBuilder = new StringBuilder();
        textBuilder.AppendLine($"Exception: {exception.Message}");
        
        if (!string.IsNullOrEmpty(queryParams))
        {
            textBuilder.AppendLine($"Query Parameters: {queryParams}");
        }
        
        if (!string.IsNullOrEmpty(bodyParams))
        {
            textBuilder.AppendLine($"Body Parameters: {bodyParams}");
        }
        
        await SaveToJournalAsync(
            serviceScopeFactory,
            eventId,
            queryParams,
            bodyParams,
            exception,
            textBuilder.ToString());
        
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        
        object response;
        
        switch (exception)
        {
            case SecureException secureEx:
                response = new
                {
                    type = secureEx.Type,
                    id = secureEx.Id,
                    data = new { message = secureEx.Message }
                };
                break;
            case DbUpdateException dbEx:
            {
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
            
                _logger.LogError(dbEx, "Database update error. Event ID: {EventId}", eventId);
            
                response = new
                {
                    type = "Exception",
                    id = eventId,
                    data = new { message = $"Internal server error ID = {eventId}" }
                };
                break;
            }
            default:
                response = new
                {
                    type = "Exception",
                    id = eventId,
                    data = new { message = $"Internal server error ID = {eventId}" }
                };
            
                _logger.LogError(exception, "Unhandled exception. Event ID: {EventId}", eventId);
                break;
        }
        
        await context.Response.WriteAsJsonAsync(response);
    }
    
    private async Task SaveToJournalAsync(
        IServiceScopeFactory serviceScopeFactory,
        long eventId,
        string? queryParams,
        string? bodyParams,
        Exception exception,
        string text)
    {
        try
        {
            using var scope = serviceScopeFactory.CreateScope();

            var journalContext = scope.ServiceProvider.GetRequiredService<JournalDbContext>();
            
            var journal = new ExceptionJournal
            {
                EventId = eventId,
                CreatedAt = DateTime.UtcNow,
                QueryParameters = queryParams,
                BodyParameters = bodyParams,
                StackTrace = exception.StackTrace ?? string.Empty,
                Text = text
            };
            
            journalContext.ExceptionJournals.Add(journal);
            await journalContext.SaveChangesAsync();
            
            _logger.LogInformation("Exception logged to journal. Event ID: {EventId}", eventId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "CRITICAL: Failed to save exception to journal. Original Event ID: {EventId}", 
                eventId);
        }
    }
}