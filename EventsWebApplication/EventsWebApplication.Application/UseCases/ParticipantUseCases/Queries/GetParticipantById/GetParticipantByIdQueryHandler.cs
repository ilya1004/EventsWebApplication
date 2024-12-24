using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Domain.Abstractions.Data;

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
            throw new NotFoundException($"Participant with ID {query.Id} not found.");
        }

        return participant;
    }
}