using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Abstractions.EmailSenderService;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobService _blobService;
    private readonly IMapper _mapper;
    private readonly IEmailSenderService _emailSenderService;

    public UpdateEventCommandHandler(IUnitOfWork unitOfWork, IBlobService blobService, IMapper mapper, IEmailSenderService emailSenderService)
    {
        _unitOfWork = unitOfWork;
        _blobService = blobService;
        _mapper = mapper;
        _emailSenderService = emailSenderService;
    }

    public async Task Handle(UpdateEventCommand command, CancellationToken cancellationToken)
    {

        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(command.Id);

        if (eventObj is null)
        {
            throw new NotFoundException($"Event with ID {command.Id} not found.");
        }

        var eventEntity = _mapper.Map<Event>(command);

        Guid? imageFileId = null;
        if (command.FileStream is not null)
        {
            if (!string.IsNullOrEmpty(eventObj.Image) && Guid.TryParse(eventObj.Image, out Guid imageId))
            {
                await _blobService.DeleteAsync(imageId, cancellationToken);
            }

            imageFileId = await _blobService.UploadAsync(
                command.FileStream,
                command.ContentType!,
                cancellationToken);
            
            eventEntity.Image = imageFileId?.ToString();
        }

        await _unitOfWork.EventsRepository.UpdateAsync(eventEntity, cancellationToken);
        await _unitOfWork.SaveAllAsync(cancellationToken);

        await _emailSenderService.SendEmailNotifications(eventObj, eventEntity, cancellationToken);
    }
}
