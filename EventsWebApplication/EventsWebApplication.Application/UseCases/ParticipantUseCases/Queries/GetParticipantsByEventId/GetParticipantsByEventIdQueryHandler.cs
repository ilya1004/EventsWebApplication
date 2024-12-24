using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Queries.GetParticipantsByEventId;

public class GetParticipantsByEventIdQueryHandler : IRequestHandler<GetParticipantsByEventIdQuery, IEnumerable<Participant>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetParticipantsByEventIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Participant>> Handle(GetParticipantsByEventIdQuery query, CancellationToken cancellationToken)
    {
        int offset = (query.PageNo - 1) * query.PageSize;

        return await _unitOfWork.ParticipantsRepository.PaginatedListAsync(p => p.EventId == query.EventId, offset, query.PageSize, cancellationToken);
    }
}
