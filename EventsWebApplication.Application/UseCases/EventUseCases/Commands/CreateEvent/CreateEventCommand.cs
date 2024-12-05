namespace EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

public sealed record CreateEventCommand(
    string Title,
    string Description,
    DateTime EventDateTime,
    int ParticipantsMaxCount,
    string? Image,
    string PlaceName,
    string? CategoryName) : IRequest;
