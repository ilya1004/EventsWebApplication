namespace EventsWebApplication.Domain.Abstractions.StartupServices;

public interface IAzuriteStartupService
{
    Task CreateContainerIfNotExistAsync();
}
