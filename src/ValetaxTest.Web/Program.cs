using System.Reflection;
using Microsoft.OpenApi.Models;
using ValetaxTest.Infrastructure.Postgres.DependencyInjection;
using ValetaxTest.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddPostgresInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Swagger",
        Version = "0.0.1",
    });
    
    options.TagActionsBy(api =>
    {
        if (api.GroupName != null) return [api.GroupName];
        
        var controllerName = api.ActionDescriptor.RouteValues["controller"];
        return [controllerName ?? "default"];
    });
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
    
    options.OrderActionsBy(api => api.RelativePath);
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();