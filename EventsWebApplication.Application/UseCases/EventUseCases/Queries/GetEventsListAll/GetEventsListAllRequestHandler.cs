namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsListAll;

internal class GetEventsListAllRequestHandler : IRequestHandler<GetEventsListAllRequest, IEnumerable<Event>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetEventsListAllRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Event>> Handle(GetEventsListAllRequest request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.EventsRepository.PaginatedListAllAsync(request.Limit, request.Offset);
    }
}
