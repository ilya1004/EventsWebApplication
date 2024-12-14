namespace EventsWebApplication.API.Contracts.Events;

public sealed record GetEventsByFilterRequest(
    DateTime? DateStart,
    DateTime? DateEnd,
    string? PlaceName,
    string? CategoryName,
    int PageNo = 1,
    int PageSize = 10);