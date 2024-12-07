namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeleteEvent;

public sealed record DeleteEventCommand(int Id) : IRequest;
