using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using EventsWebApplication.Domain.Abstractions.StartupServices;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Abstractions.BlobStorage;

namespace EventsWebApplication.Infrastructure.StartupServices;

public class AzuriteStartupService : IAzuriteStartupService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IBlobService _blobService;
    private readonly string _imagesPath;

    public AzuriteStartupService(IServiceProvider serviceProvider, IConfiguration configuration, IUnitOfWork unitOfWork, BlobServiceClient blobServiceClient, IBlobService blobService)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _blobServiceClient = blobServiceClient;
        _blobService = blobService;
        _imagesPath = Path.Combine(AppContext.BaseDirectory, "images");
    }

    public async Task CreateContainerIfNotExistAsync()
    {
        var containerName = _configuration["BlobStorage:ContainerName"] ?? "images";
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        await containerClient.CreateIfNotExistsAsync(
            PublicAccessType.Blob,
            metadata: null
        );
    }

    public async Task SeedImagesAsync(CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(_imagesPath))
        {
            return;
        }

        var eventNumber = 1;
        foreach (var filePath in Directory.GetFiles(_imagesPath).Order())
        {
            var eventEntity = await _unitOfWork.EventsRepository.FirstOrDefaultAsync(e => e.Id == eventNumber, cancellationToken);
            eventNumber++;

            if (eventEntity is not null && eventEntity.Image is null)
            {
                var fileName = Path.GetFileName(filePath);
                var contentType = "image/jpeg";
                using var stream = File.OpenRead(filePath);
                var imageId = await _blobService.UploadAsync(stream, contentType, cancellationToken);

                eventEntity.Image = imageId.ToString();
                await _unitOfWork.EventsRepository.UpdateAsync(eventEntity, cancellationToken);
            }
        }
        await _unitOfWork.SaveAllAsync(cancellationToken);
    }
}
