using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Abstractions.UserInfoProvider;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.AddParticipantToEvent;

public class AddParticipantToEventCommandHandler : IRequestHandler<AddParticipantToEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserInfoProvider _userInfoProvider;

    public AddParticipantToEventCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserInfoProvider userInfoProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userInfoProvider = userInfoProvider;
    }

    public async Task Handle(AddParticipantToEventCommand command, CancellationToken cancellationToken)
    {
        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(command.EventId, cancellationToken, e => e.Participants);

        if (eventObj == null)
        {
            throw new NotFoundException($"Event with given ID {command.EventId} not found.");
        }

        if (eventObj.Participants.Count == eventObj.ParticipantsMaxCount)
        {
            throw new BadRequestException($"Event has reached the maximum number of participants.");
        }

        var userInfoResponse = await _userInfoProvider.GetUserInfoAsync(command.UserId, command.Token, cancellationToken);

        var userInfo = _mapper.Map<UserInfoDTO>(userInfoResponse);

        var isAlreadyParticipate = await _unitOfWork.ParticipantsRepository.AnyAsync(
            p => p.Email == userInfo.Email && p.EventId == command.EventId, 
            cancellationToken);

        if (isAlreadyParticipate)
        {
            throw new AlreadyExistsException("You are alredy participating in this event");
        }

        var participant = _mapper.Map<Participant>(userInfo);

        participant.Event = eventObj;
        participant.EventRegistrationDate = DateTime.UtcNow;

        await _unitOfWork.ParticipantsRepository.AddAsync(participant, cancellationToken);
        await _unitOfWork.SaveAllAsync(cancellationToken);
    }
}
