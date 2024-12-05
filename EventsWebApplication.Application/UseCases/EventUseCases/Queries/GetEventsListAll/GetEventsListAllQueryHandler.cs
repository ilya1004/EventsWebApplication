namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsListAll;

internal class GetEventsListAllQueryHandler : IRequestHandler<GetEventsListAllQuery, IEnumerable<Event>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetEventsListAllQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Event>> Handle(GetEventsListAllQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.EventsRepository.PaginatedListAllAsync(request.Limit, request.Offset, cancellationToken);
    }
}
