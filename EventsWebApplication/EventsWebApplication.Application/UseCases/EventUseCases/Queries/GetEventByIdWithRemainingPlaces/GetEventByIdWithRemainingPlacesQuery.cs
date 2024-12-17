using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventByIdWithRemainingPlaces;

public sealed record GetEventByIdWithRemainingPlacesQuery(int EventId) : IRequest<EventWithRemainingPlacesDTO>;
