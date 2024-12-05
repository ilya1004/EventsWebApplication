namespace EventsWebApplication.API.Contracts.Events;

public sealed record CreateEventRequest(
    string Title,
    string? Description,
    DateTime EventDateTime,
    int ParticipantsMaxCount,
    string? Image,
    string PlaceName,
    string? CategoryName);
