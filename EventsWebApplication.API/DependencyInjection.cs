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

        return services;
    }
}
