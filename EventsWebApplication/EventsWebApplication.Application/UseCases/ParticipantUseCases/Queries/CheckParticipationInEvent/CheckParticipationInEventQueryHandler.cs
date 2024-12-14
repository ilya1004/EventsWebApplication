
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Queries.CheckParticipationInEvent;

public class CheckParticipationInEventQueryHandler : IRequestHandler<CheckParticipationInEventQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckParticipationInEventQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CheckParticipationInEventQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ParticipantsRepository.AnyAsync(p => p.Email == request.Email && p.EventId == request.EventId, cancellationToken);

        return result;
    }
}
