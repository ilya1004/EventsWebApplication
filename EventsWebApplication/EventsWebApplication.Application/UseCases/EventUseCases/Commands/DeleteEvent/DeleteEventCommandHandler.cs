﻿using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeleteEvent;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobService _blobService;

    public DeleteEventCommandHandler(IUnitOfWork unitOfWork, IBlobService blobService)
    {
        _unitOfWork = unitOfWork;
        _blobService = blobService;
    }

    public async Task Handle(DeleteEventCommand command, CancellationToken cancellationToken)
    {
        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(command.Id);

        if (eventObj is null)
        {
            throw new NotFoundException($"Event with ID {command.Id} not found.");
        }

        if (!string.IsNullOrEmpty(eventObj.Image) && Guid.TryParse(eventObj.Image, out Guid imageId))
        {
            await _blobService.DeleteAsync(imageId, cancellationToken);
        }

        await _unitOfWork.EventsRepository.DeleteAsync(eventObj, cancellationToken);
        await _unitOfWork.SaveAllAsync(cancellationToken);
    }
}
