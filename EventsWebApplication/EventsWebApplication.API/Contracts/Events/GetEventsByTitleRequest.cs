namespace EventsWebApplication.API.Contracts.Events;

public sealed record GetEventsByTitleRequest(string TitleQuery, int PageNo = 1, int PageSize = 10);
