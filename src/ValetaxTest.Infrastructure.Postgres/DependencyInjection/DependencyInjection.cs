using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Valetax.Application.Trees;
using ValetaxTest.Infrastructure.Postgres.Repositories;

namespace ValetaxTest.Infrastructure.Postgres.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ApplicationDbContext>(_ => 
            new ApplicationDbContext(configuration.GetConnectionString("ValetaxTestDb")!));

        services.AddScoped<ITreesRepository, TreesRepository>();
        
        return services;
    }
}