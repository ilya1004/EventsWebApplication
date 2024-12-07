namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByFilter;

public sealed record GetEventsByFilterQuery(
    DateTime? DateStart,
    DateTime? DateEnd, 
    string? PlaceName, 
    string? CategoryName,
    int PageNo = 1,
    int PageSize = 10) : IRequest<IEnumerable<Event>>;
