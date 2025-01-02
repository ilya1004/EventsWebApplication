using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByCurrentUser;

public class GetEventsByCurrentUserQueryHandler : IRequestHandler<GetEventsByCurrentUserQuery, IEnumerable<Event>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEventsByCurrentUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Event>> Handle(GetEventsByCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var myParticipations = await _unitOfWork.ParticipantsRepository.ListAsync(p => p.Email == request.Email, cancellationToken);

        int offset = (request.PageNo - 1) * request.PageSize;
        var result = await _unitOfWork.EventsRepository.PaginatedListAsync(
            e => myParticipations
                .Select(p => p.EventId)
                .Any(ids => ids == e.Id), 
            offset, 
            request.PageSize, 
            cancellationToken);

        return result;
    }
}
