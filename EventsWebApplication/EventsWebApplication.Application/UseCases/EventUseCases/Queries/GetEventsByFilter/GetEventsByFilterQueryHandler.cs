using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByFilter;

public class GetEventsByFilterQueryHandler : IRequestHandler<GetEventsByFilterQuery, IEnumerable<Event>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEventsByFilterQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Event>> Handle(GetEventsByFilterQuery query, CancellationToken cancellationToken)
    {
        int offset = (query.PageNo - 1) * query.PageSize;

        DateTime? dateStart = query.DateStart is not null ? new DateTime(query.DateStart.Value.Ticks, DateTimeKind.Utc) : null;
        DateTime? dateEnd = query.DateEnd is not null ? new DateTime(query.DateEnd.Value.Ticks, DateTimeKind.Utc) : null;

        var result = await _unitOfWork.EventsRepository.GetByFilterAsync(
            dateStart,
            dateEnd,
            query.PlaceName, 
            query.CategoryName, 
            offset, 
            query.PageSize, 
            cancellationToken);

        return result;
    }
}
