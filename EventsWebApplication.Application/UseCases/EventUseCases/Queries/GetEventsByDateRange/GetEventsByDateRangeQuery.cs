namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDateRange;

public sealed record GetEventsByDateRangeQuery(DateTime DateStart, DateTime DateEnd, int PageNo = 1, int PageSize = 10) : IRequest<IEnumerable<Event>>;
