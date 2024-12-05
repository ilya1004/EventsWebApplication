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
        int offset = (request.PageNo - 1) * request.PageSize;

        return await _unitOfWork.EventsRepository.PaginatedListAllAsync(offset, request.PageSize, cancellationToken);
    }
}
