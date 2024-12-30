using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace EventsWebApplication.API.Extensions;

public static class AzuriteExtension
{
    public static void CreateContainerIfNotExist(this IApplicationBuilder app, IConfiguration configuration, CancellationToken cancellationToken = default)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var blobService = scope.ServiceProvider.GetRequiredService<BlobServiceClient>();
        var containerName = configuration["BlobStorage:ContainerName"] ?? "images";
        var containerClient = blobService.GetBlobContainerClient(containerName);

        containerClient.CreateIfNotExists(
            PublicAccessType.Blob,
            metadata: null,
            cancellationToken: cancellationToken
        );

        Console.WriteLine($"Container '{containerName}' successfully created or already exists.");
    }
}
