using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EventsWebApplication.Domain.Abstractions.BlobStorage;

namespace EventsWebApplication.Infrastructure.BlobStorage;

internal class BlobService : IBlobService
{
    private const string ContainerName = "images";
    private readonly BlobServiceClient _blobServiceClient;
    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Guid> UploadAsync(Stream stream, string contentType, CancellationToken cancellationToken = default)
    {
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

        var fileId = new Guid();

        BlobClient blobClient = blobContainerClient.GetBlobClient(fileId.ToString());

        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);

        return fileId;
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

        BlobClient blobClient = blobContainerClient.GetBlobClient(fileId.ToString());

        Response<BlobDownloadResult> fileResponse = await blobClient.DownloadContentAsync(cancellationToken);
        
        return new FileResponse(fileResponse.Value.Content.ToStream(), fileResponse.Value.Details.ContentType);
    }

    public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

        BlobClient blobClient = blobContainerClient.GetBlobClient(fileId.ToString());

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}
