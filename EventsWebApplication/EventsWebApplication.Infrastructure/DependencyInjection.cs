using Azure.Storage.Blobs;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Abstractions.EmailSenderService;
using EventsWebApplication.Domain.Abstractions.StartupServices;
using EventsWebApplication.Domain.Abstractions.UserInfoProvider;
using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.Repository;
using EventsWebApplication.Infrastructure.Services.BlobStorage;
using EventsWebApplication.Infrastructure.Services.EmailService;
using EventsWebApplication.Infrastructure.Services.IdentityServerApiAccessor;
using EventsWebApplication.Infrastructure.StartupServices;
using Microsoft.EntityFrameworkCore;
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
        services.AddScoped<IEmailSenderService, EmailSenderService>();

        services.AddScoped<IAzuriteStartupService, AzuriteStartupService>();
        services.AddScoped<IMigrationsStartupService, MigrationsStartupService>();

        return services;
    }
}
