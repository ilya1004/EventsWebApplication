using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Entities.Participants;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Queries.GetParticipantById;

public class GetParticipantByIdQueryHandler : IRequestHandler<GetParticipantByIdQuery, Participant>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetParticipantByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Participant> Handle(GetParticipantByIdQuery query, CancellationToken cancellationToken)
    {
        var participant = await _unitOfWork.ParticipantsRepository.GetByIdAsync(query.Id, cancellationToken);

        if (participant == null) 
        {
            throw new Exception($"Participant with ID {query.Id} not found.");
        }

        return participant;
    }
}