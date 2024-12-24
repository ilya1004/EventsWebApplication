using EventsAppIdentityServer.Domain.Abstractions;
using EventsAppIdentityServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsAppIdentityServer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgresIdentityConnection")));

        services.AddScoped<IDbInitializer, DbInitializer>();

        return services;
    }
}
