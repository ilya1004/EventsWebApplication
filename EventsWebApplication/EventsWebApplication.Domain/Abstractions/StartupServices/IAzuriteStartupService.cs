namespace EventsWebApplication.Domain.Abstractions.StartupServices;

public interface IAzuriteStartupService
{
    Task CreateContainerIfNotExistAsync();

    Task SeedImagesAsync(CancellationToken cancellationToken = default);
}
