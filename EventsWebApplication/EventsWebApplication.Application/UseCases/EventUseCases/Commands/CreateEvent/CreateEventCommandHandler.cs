
using AutoMapper;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand>
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

    public async Task Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.EventsRepository.IsSameEventExists(command.EventDTO.Title, 
                                                                 command.EventDTO.EventDateTime, 
                                                                 command.EventDTO.PlaceName,
                                                                 cancellationToken))
        {
            throw new AlreadyExistsException("Event with this Title, DateTime and Place already exists");
        }

        Guid? imageFileId = null;

        if (command.FileStream != null)
        {
            imageFileId = await _blobService.UploadAsync(
                command.FileStream,
                command.ContentType!,
                cancellationToken);
        }

        var eventEntity = _mapper.Map<Event>(command);

        eventEntity.Image = imageFileId?.ToString();

        await _unitOfWork.EventsRepository.AddAsync(eventEntity, cancellationToken);
        await _unitOfWork.SaveAllAsync(cancellationToken);
    }
}
