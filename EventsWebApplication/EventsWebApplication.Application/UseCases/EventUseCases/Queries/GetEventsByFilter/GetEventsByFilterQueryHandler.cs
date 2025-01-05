using EventsWebApplication.Application.Specifications.EventSpecifications;
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

        var specification = new EventsListByFilterSpecification(
            query.DateStart,
            query.DateEnd,
            query.PlaceName,
            query.CategoryName,
            offset,
            query.PageSize);

        var result = await _unitOfWork.EventsRepository.GetByFilterAsync(specification, cancellationToken);

        return result;
    }
}
