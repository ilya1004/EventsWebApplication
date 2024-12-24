using EventsAppIdentityServer.Domain.Abstractions;
using EventsAppIdentityServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventsAppIdentityServer.API.Extensions;

public static class DatabaseExtension
{
    public static void MakeMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }

    public static async Task SeedDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

        await dbInitializer.InitializeDb();
    }
}
