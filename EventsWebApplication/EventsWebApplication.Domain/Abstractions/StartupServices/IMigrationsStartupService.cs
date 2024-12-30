namespace EventsWebApplication.Domain.Abstractions.StartupServices;

public interface IMigrationsStartupService
{
    Task MakeMigrationsAsync();
}
