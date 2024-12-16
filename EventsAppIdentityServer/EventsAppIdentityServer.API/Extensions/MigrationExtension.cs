using EventsAppIdentityServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventsAppIdentityServer.API.Extensions;

public static class MigrationExtension
{
    public static void MakeMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}
