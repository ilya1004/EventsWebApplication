namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsListAll;

public sealed record GetEventsListAllQuery(int PageNo = 1, int PageSize = 10) : IRequest<IEnumerable<Event>>;
