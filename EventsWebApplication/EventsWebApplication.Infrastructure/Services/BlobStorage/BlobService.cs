using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Domain.Abstractions.BlobStorage;

namespace EventsWebApplication.Infrastructure.Services.BlobStorage;

public class BlobService : IBlobService
{
    private const string ContainerName = "images";
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Guid> UploadAsync(Stream stream, string contentType, CancellationToken cancellationToken = default)
    {
        try
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

            var fileId = Guid.NewGuid();
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileId.ToString());

            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);

            return fileId;
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.ContainerNotFound)
        {
            throw new NotFoundException("The specified blob container does not exist.");
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"An error occurred during the upload process: {ex.Message}");
        }
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        try
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

            BlobClient blobClient = blobContainerClient.GetBlobClient(fileId.ToString());

            Response<BlobDownloadResult> fileResponse = await blobClient.DownloadContentAsync(cancellationToken);

            return new FileResponse(fileResponse.Value.Content.ToStream(), fileResponse.Value.Details.ContentType);
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            throw new NotFoundException($"The file with ID {fileId} was not found in the blob storage.");
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"An error occurred during the download process: {ex.Message}");
        }
    }

    public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        try
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

            BlobClient blobClient = blobContainerClient.GetBlobClient(fileId.ToString());

            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            throw new NotFoundException($"The file with ID {fileId} was not found in the blob storage.");
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"An error occurred during the deletion process: {ex.Message}");
        }
    }
}
