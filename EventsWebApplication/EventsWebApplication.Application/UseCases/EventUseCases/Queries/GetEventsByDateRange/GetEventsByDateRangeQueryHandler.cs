using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDateRange;

public class GetEventsByDateRangeQueryHandler : IRequestHandler<GetEventsByDateRangeQuery, IEnumerable<Event>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetEventsByDateRangeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Event>> Handle(GetEventsByDateRangeQuery request, CancellationToken cancellationToken)
    {
        int offset = (request.PageNo - 1) * request.PageSize;

        return await _unitOfWork.EventsRepository.PaginatedListAsync(
            e => request.DateStart.Date <= e.EventDateTime.Date && e.EventDateTime.Date <= request.DateEnd.Date,
            offset,
            request.PageSize,
            cancellationToken);
    }
}
