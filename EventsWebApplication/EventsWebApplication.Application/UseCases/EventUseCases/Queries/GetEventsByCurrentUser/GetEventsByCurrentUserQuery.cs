namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByCurrentUser;

public sealed record GetEventsByCurrentUserQuery(string Email, int PageNo = 1, int PageSize = 10) : IRequest<IEnumerable<Event>>;
