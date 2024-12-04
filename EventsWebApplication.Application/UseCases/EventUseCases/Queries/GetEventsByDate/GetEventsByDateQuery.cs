namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDate;

public sealed record GetEventsByDateQuery(DateTime Date, int PageNo = 1, int PageSize = 10) : IRequest<IEnumerable<Event>>;
