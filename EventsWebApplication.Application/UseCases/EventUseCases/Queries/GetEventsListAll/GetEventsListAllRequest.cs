namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsListAll;

public sealed record GetEventsListAllRequest(int Offset, int Limit) : IRequest<IEnumerable<Event>>;
