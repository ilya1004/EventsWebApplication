using FluentValidation;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace EventsAppIdentityServer.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation(cfg =>
        {
            cfg.EnableFormBindingSourceAutomaticValidation = true;
            cfg.EnableBodyBindingSourceAutomaticValidation = true;
        });
        
        return services;
    }
}
