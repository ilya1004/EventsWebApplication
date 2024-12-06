
using AutoMapper;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

internal class CreateEventCommandHandler : IRequestHandler<CreateEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBlobService _blobService;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.EventsRepository.IsSameEventExists(request.EventDTO.Title, 
                                                                 request.EventDTO.EventDateTime, 
                                                                 request.EventDTO.PlaceName))
        {
            throw new Exception("Event with this Title, DateTime and Place already exists");
        }

        Guid? imageFileId = null;

        if (request.EventDTO.ImageFile != null)
        {
            using var stream = request.EventDTO.ImageFile.OpenReadStream();

            imageFileId = await _blobService.UploadAsync(
                stream,
                request.EventDTO.ImageFile.ContentType,
                cancellationToken);
        }

        var eventEntity = _mapper.Map<Event>(request.EventDTO);

        eventEntity.Image = imageFileId.ToString();

        await _unitOfWork.EventsRepository.AddAsync(eventEntity, cancellationToken);

        await _unitOfWork.SaveAllAsync(cancellationToken);
    }
}
