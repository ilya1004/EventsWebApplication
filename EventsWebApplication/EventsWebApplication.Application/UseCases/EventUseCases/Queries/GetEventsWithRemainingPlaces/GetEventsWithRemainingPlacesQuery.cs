using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsWithRemainingPlaces;

public sealed record GetEventsWithRemainingPlacesQuery(int PageNo = 1, int PageSize = 10) : IRequest<IEnumerable<EventWithRemainingPlacesDTO>>;
