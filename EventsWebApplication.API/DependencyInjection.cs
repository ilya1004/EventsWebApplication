using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

namespace EventsWebApplication.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(Assembly.GetExecutingAssembly());

            cfg.AddMaps(typeof(Application.DependencyInjection).Assembly);
        });

        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation(cfg =>
        {
            cfg.EnableFormBindingSourceAutomaticValidation = true;
            cfg.EnableBodyBindingSourceAutomaticValidation = true;
        });


        return services;
    }
}
