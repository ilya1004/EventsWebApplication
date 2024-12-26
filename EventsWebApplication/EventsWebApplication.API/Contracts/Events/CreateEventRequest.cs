using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.API.Contracts.Events;

public sealed record CreateEventRequest(
    EventDTO EventDTO, 
    IFormFile? ImageFile);
