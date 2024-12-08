
using EventsWebApplication.Domain.Abstractions.Data;
using MediatR;

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

        var result = await _unitOfWork.EventsRepository.GetByFilterAsync(query.DateStart, query.DateEnd, query.PlaceName, query.CategoryName, offset, query.PageSize, cancellationToken);

        return result;
    }

}
