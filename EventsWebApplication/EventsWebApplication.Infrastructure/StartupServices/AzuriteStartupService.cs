using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventsWebApplication.Domain.Abstractions.StartupServices;

namespace EventsWebApplication.Infrastructure.StartupServices;

public class AzuriteStartupService : IAzuriteStartupService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public AzuriteStartupService(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public async Task CreateContainerIfNotExistAsync()
    {
        using var scope = _serviceProvider.CreateScope();

        var blobService = scope.ServiceProvider.GetRequiredService<BlobServiceClient>();
        var containerName = _configuration["BlobStorage:ContainerName"] ?? "images";
        var containerClient = blobService.GetBlobContainerClient(containerName);

        await containerClient.CreateIfNotExistsAsync(
            PublicAccessType.Blob,
            metadata: null
        );
    }
}
