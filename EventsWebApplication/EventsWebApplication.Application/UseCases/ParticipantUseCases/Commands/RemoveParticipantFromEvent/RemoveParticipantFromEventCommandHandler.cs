using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.RemoveParticipantFromEvent;

public class RemoveParticipantFromEventCommandHandler : IRequestHandler<RemoveParticipantFromEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveParticipantFromEventCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(RemoveParticipantFromEventCommand command, CancellationToken cancellationToken)
    {
        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(command.EventId);

        if (eventObj == null)
        {
            throw new NotFoundException($"Event with given ID {command.EventId} not found.");
        }

        var participant = await _unitOfWork.ParticipantsRepository.FirstOrDefaultAsync(p => p.Email == command.Email && p.EventId == command.EventId, cancellationToken);

        if (participant == null)
        {
            throw new NotFoundException($"Participant with given Email {command.Email} not found.");
        }

        await _unitOfWork.ParticipantsRepository.DeleteAsync(participant, cancellationToken);
        await _unitOfWork.SaveAllAsync(cancellationToken);
    }
}
