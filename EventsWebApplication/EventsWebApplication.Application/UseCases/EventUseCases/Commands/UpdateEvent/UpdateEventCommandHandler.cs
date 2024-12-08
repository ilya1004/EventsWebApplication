
using AutoMapper;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;
using MediatR;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobService _blobService;
    private readonly IMapper _mapper;

    public UpdateEventCommandHandler(IUnitOfWork unitOfWork, IBlobService blobService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _blobService = blobService;
        _mapper = mapper;
    }
    public async Task Handle(UpdateEventCommand command, CancellationToken cancellationToken)
    {

        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(command.Id);

        if (eventObj == null)
        {
            throw new Exception($"Event with ID {command.Id} not found.");
        }

        Guid? imageFileId = null;
        if (command.EventDTO.ImageFile != null)
        {
            if (eventObj.Image != null)
            {
                await _blobService.DeleteAsync(new Guid(eventObj.Image), cancellationToken);
            }
            
            using var stream = command.EventDTO.ImageFile.OpenReadStream();
            
            imageFileId = await _blobService.UploadAsync(
                stream,
                command.EventDTO.ImageFile.ContentType,
                cancellationToken);
        }

        var eventEntity = _mapper.Map<Event>(command.EventDTO);

        eventEntity.Image = imageFileId.ToString();
        
        eventEntity.Id = command.Id;

        await _unitOfWork.EventsRepository.UpdateAsync(eventEntity, cancellationToken);
    }
}
