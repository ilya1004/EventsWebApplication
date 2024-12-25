﻿using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Abstractions.EmailSenderService;
using FluentEmail.Core;

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

        var newEventEntity = _mapper.Map<Event>(command.EventDTO);

        await _emailSenderService.SendEmailNotifications(eventEntity, newEventEntity, cancellationToken);
    }
}
