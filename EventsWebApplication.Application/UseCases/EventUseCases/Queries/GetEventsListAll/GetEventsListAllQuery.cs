namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsListAll;

public sealed record GetEventsListAllQuery(int Offset, int Limit) : IRequest<IEnumerable<Event>>;
