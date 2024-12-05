using EventsWebApplication.Domain.Entities.Events;

namespace EventsWebApplication.Domain.Abstractions;

public interface IEventsRepository : IRepository<Event>
{
    public Task<bool> IsSameEventExists(string title, DateTime dateTime, string placeName);
}
