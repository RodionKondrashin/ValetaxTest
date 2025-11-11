using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ValetaxTest.Infrastructure.Postgres.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ApplicationDbContext>(_ => 
            new ApplicationDbContext(configuration.GetConnectionString("ValetaxTestDb")!));
        
        return services;
    }
}