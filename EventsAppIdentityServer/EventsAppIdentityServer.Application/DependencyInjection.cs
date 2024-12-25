using Duende.IdentityServer.Services;
using EventsAppIdentityServer.Application.Services;
using EventsAppIdentityServer.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventsAppIdentityServer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddAutoMapper(config =>
        {
            config.AddMaps(Assembly.GetExecutingAssembly());
        });

        services.AddTransient<IProfileService, MyProfileService>();
        services.AddScoped<ITokenValidationService, TokenValidationService>();

        return services;
    }
}
