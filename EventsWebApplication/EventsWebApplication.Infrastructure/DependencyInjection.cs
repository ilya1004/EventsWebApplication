using Azure.Storage.Blobs;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Abstractions.UserInfoProvider;
using EventsWebApplication.Infrastructure.BlobStorage;
using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.IdentityServerApiAccessor;
using EventsWebApplication.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsWebApplication.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringPostgres = configuration.GetConnectionString("PostgresConnection") ?? "Host=localhost; Port=5433; Database=eventsappdb; Username=postgres; Password=passpass";
        var connectionStringAzurite = configuration.GetConnectionString("AzuriteConnection") ?? "UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://eventwebapp.azurite;";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionStringPostgres),
            ServiceLifetime.Scoped);

        services.AddScoped<IUnitOfWork, AppUnitOfWork>();

        services.AddSingleton<IBlobService, BlobService>();
        services.AddSingleton(_ => new BlobServiceClient(connectionStringAzurite));

        services.AddScoped<IUserInfoProvider, UserInfoProvider>();

        return services;
    }
}
