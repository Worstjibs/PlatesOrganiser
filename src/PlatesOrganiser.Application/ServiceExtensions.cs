using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PlatesOrganiser.Application.Behaviours;
using System.Reflection;

namespace PlatesOrganiser.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

        return services;
    }
}
