using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using HardwareCatalog.Application.Behaviors;
using System.Reflection;

namespace HardwareCatalog.Application.Extensions;

/// <summary>
/// Extension methods for registering Application layer services in the DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            
            // Register the validation behavior
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
