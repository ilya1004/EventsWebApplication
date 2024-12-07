using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.API.Contracts.Events;

public sealed record UpdateEventRequest(int Id, EventDTO EventDTO);
