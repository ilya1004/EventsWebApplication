namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByTitle;

public sealed record GetEventsByTitleRequest(string TitleQuery, int PageNo = 1, int PageSize = 10) : IRequest<IEnumerable<Event>>;
