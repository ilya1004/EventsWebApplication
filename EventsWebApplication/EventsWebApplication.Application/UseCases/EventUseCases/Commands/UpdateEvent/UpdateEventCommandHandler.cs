using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;
using FluentEmail.Core;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobService _blobService;
    private readonly IMapper _mapper;
    private readonly IFluentEmail _fluentEmail;

    public UpdateEventCommandHandler(IUnitOfWork unitOfWork, IBlobService blobService, IMapper mapper, IFluentEmail fluentEmail)
    {
        _unitOfWork = unitOfWork;
        _blobService = blobService;
        _mapper = mapper;
        _fluentEmail = fluentEmail;
    }
    public async Task Handle(UpdateEventCommand command, CancellationToken cancellationToken)
    {

        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(command.Id);

        if (eventObj == null)
        {
            throw new NotFoundException($"Event with ID {command.Id} not found.");
        }

        Guid? imageFileId = null;
        if (command.EventDTO.ImageFile != null)
        {
            if (!string.IsNullOrEmpty(eventObj.Image) && Guid.TryParse(eventObj.Image, out Guid imageId))
            {
                await _blobService.DeleteAsync(imageId, cancellationToken);
            }
            
            using var stream = command.EventDTO.ImageFile.OpenReadStream();
            
            imageFileId = await _blobService.UploadAsync(
                stream,
                command.EventDTO.ImageFile.ContentType,
                cancellationToken);
        }

        var eventEntity = _mapper.Map<Event>(command.EventDTO);

        eventEntity.Image = imageFileId?.ToString();
        
        eventEntity.Id = command.Id;

        await _unitOfWork.EventsRepository.UpdateAsync(eventEntity, cancellationToken);
        await _unitOfWork.SaveAllAsync(cancellationToken);

        var eventDateTimeChanged = eventObj.EventDateTime != command.EventDTO.EventDateTime;
        var eventPlaceChanged = eventObj.Place.Name != command.EventDTO.PlaceName;

        if (!eventDateTimeChanged && !eventDateTimeChanged)
        {
            return;
        }

        var eventParticipants = await _unitOfWork.ParticipantsRepository.ListAsync(p => p.EventId == eventEntity.Id);

        foreach (var participant in eventParticipants)
        {
            var message = BuildNotificationMessage(eventDateTimeChanged, eventPlaceChanged, command.EventDTO);

            if (!string.IsNullOrEmpty(message))
            {
                await _fluentEmail
                    .To(participant.Email)
                    .Subject($"Changes in event: {eventEntity.Title}")
                    .Body(message, isHtml: true)
                    .SendAsync(cancellationToken);
            }
        }
    }

    private string BuildNotificationMessage(bool dateTimeChanged, bool placeChanged, EventDTO eventDTO)
    {
        var changesHtml = new List<string>();

        if (dateTimeChanged)
        {
            changesHtml.Add($"<li>The event date and time has been changed to <b>{eventDTO.EventDateTime:dd.MM.yyyy, HH:mm}</b>.</li>");
        }

        if (placeChanged)
        {
            changesHtml.Add($"<li>The event place has been updated to <b>{eventDTO.PlaceName}</b>.</li>");
        }

        var body = $@"
            <p>Dear Event Participant</p>
            <p>We would like to inform you about some updates regarding the event <b>{eventDTO.Title}</b>:</p>
            <ul>
                {string.Join("\n", changesHtml)}
            </ul>
            <p>Thank you for your attention!</p>
            <p>Your EventsWebApplication Team</p>";

        return body;
    }
}
