namespace EventsWebApplication.API.Contracts.Events;

internal sealed record UpdateEventRequest(
    string Title,
    string? Description,
    DateTime EventDateTime,
    int ParticipantsMaxCount,
    string? Image,
    string PlaceName,
    string? CategoryName);
