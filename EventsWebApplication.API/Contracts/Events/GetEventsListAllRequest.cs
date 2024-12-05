namespace EventsWebApplication.API.Contracts.Events;

public sealed record GetEventsListAllRequest(int PageNo = 1, int PageSize = 10);
