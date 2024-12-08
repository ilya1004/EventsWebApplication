namespace EventsWebApplication.API.Contracts.Events;

public sealed record GetEventsByDateRequest(DateTime Date, int PageNo = 1, int PageSize = 10);
