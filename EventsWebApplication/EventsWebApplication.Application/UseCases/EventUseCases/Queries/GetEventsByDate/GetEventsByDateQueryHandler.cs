
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDate;

internal class GetEventsByDateQueryHandler : IRequestHandler<GetEventsByDateQuery, IEnumerable<Event>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetEventsByDateQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Event>> Handle(GetEventsByDateQuery request, CancellationToken cancellationToken)
    {
        int offset = (request.PageNo - 1) * request.PageSize;

        return await _unitOfWork.EventsRepository.PaginatedListAsync(
            e => e.EventDateTime.Date == request.Date.Date,
            offset,
            request.PageSize,
            cancellationToken);
    }
}
