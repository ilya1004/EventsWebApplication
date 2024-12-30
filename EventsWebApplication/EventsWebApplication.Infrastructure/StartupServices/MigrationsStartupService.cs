using EventsWebApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EventsWebApplication.Domain.Abstractions.StartupServices;

namespace EventsWebApplication.Infrastructure.StartupServices;

public class MigrationsStartupService : IMigrationsStartupService
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationsStartupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MakeMigrationsAsync()
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
