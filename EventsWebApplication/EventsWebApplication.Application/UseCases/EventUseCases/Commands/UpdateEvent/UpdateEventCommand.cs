using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;

public sealed record UpdateEventCommand(int Id, EventDTO EventDTO) : IRequest;