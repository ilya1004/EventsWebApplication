
namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByTitle;

internal class GetEventsByTitleQueryHandler : IRequestHandler<GetEventsByTitleQuery, IEnumerable<Event>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetEventsByTitleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Event>> Handle(GetEventsByTitleQuery request, CancellationToken cancellationToken)
    {
        int offset = (request.PageNo - 1) * request.PageSize;

        return await _unitOfWork.EventsRepository.PaginatedListAsync(
            e => e.Title.Contains(request.TitleQuery, StringComparison.CurrentCultureIgnoreCase), 
            offset, 
            request.PageSize, 
            cancellationToken);
    }
}
