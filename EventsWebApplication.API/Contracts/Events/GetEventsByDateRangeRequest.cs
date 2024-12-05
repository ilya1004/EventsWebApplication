namespace EventsWebApplication.API.Contracts.Events;

public sealed record GetEventsByDateRangeRequest(DateTime DateStart, DateTime DateEnd, int PageNo = 1, int PageSize = 10);
