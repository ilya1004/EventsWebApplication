using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventImageByEventId;

internal class GetEventImageByEventIdQueryHandler : IRequestHandler<GetEventImageByEventIdQuery, FileResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobService _blobService;
    private readonly IMapper _mapper;

    public GetEventImageByEventIdQueryHandler(IUnitOfWork unitOfWork, IBlobService blobService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _blobService = blobService;
        _mapper = mapper;
    }

    public async Task<FileResponseDTO> Handle(GetEventImageByEventIdQuery request, CancellationToken cancellationToken)
    {
        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(request.EventId);

        if (eventObj == null)
        {
            throw new Exception($"Event with ID {request.EventId} not found.");
        }

        if (eventObj.Image == null)
        {
            throw new Exception($"Event with ID {request.EventId} don't have an image");
        }

        if (!Guid.TryParse(eventObj.Image, out var imageId))
        {
            throw new Exception($"Event with ID {request.EventId} have an incorrect format of the image name");
        }

        var result = await _blobService.DownloadAsync(imageId, cancellationToken);

        return _mapper.Map<FileResponseDTO>(result);
    }
}
