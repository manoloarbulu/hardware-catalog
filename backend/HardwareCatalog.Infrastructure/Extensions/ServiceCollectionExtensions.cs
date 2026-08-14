using HardwareCatalog.Infrastructure.Persistence;
using HardwareCatalog.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareCatalog.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering Infrastructure layer services in the DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Register Data Seeder
        services.AddScoped<DataSeeder>();

        return services;
    }
}
