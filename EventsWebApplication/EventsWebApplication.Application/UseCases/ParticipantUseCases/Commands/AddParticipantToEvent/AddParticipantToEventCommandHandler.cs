using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.AddParticipantToEvent;

public class AddParticipantToEventCommandHandler : IRequestHandler<AddParticipantToEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddParticipantToEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(AddParticipantToEventCommand command, CancellationToken cancellationToken)
    {
        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(command.EventId, cancellationToken, e => e.Participants);

        if (eventObj == null)
        {
            throw new NotFoundException($"Event with given ID {command.EventId} not found.");
        }

        if (eventObj.Participants.Count >= eventObj.ParticipantsMaxCount)
        {
            throw new BadRequestException($"Event has reached the maximum number of participants.");
        }

        var isAlreadyParticipate = await _unitOfWork.ParticipantsRepository.AnyAsync(
            p => p.Email == command.Email && p.EventId == command.EventId, 
            cancellationToken);

        if (isAlreadyParticipate)
        {
            throw new AlreadyExistsException("You are alredy participating in this event");
        }

        var participant = _mapper.Map<Participant>(command.UserInfoDTO);
        participant.Email = command.Email;
        participant.EventId = eventObj.Id;

        await _unitOfWork.ParticipantsRepository.AddAsync(participant, cancellationToken);
        await _unitOfWork.SaveAllAsync(cancellationToken);
    }
}
