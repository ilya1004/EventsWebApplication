
namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByTitle;

internal class GetEventsByTitleRequestHandler : IRequestHandler<GetEventsByTitleRequest, IEnumerable<Event>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetEventsByTitleRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Event>> Handle(GetEventsByTitleRequest request, CancellationToken cancellationToken)
    {
        int offset = (request.PageNo - 1) * request.PageSize;

        return await _unitOfWork.EventsRepository.PaginatedListAsync(e => e.Title.Contains(request.TitleQuery, StringComparison.CurrentCultureIgnoreCase), offset, request.PageSize);
    }
}
