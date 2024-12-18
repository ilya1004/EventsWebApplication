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

        if (eventObj == null)
        {
            throw new Exception($"Event with ID {command.Id} not found.");
        }

        await _unitOfWork.EventsRepository.DeleteAsync(eventObj, cancellationToken);
        await _unitOfWork.SaveAllAsync(cancellationToken);
    }
}
