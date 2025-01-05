using EventsWebApplication.Domain.Abstractions.Specification;
using EventsWebApplication.Domain.Entities.Events;

namespace EventsWebApplication.Domain.Abstractions.Data;

public interface IEventsRepository : IRepository<Event>
{
    Task<List<Event>> GetByFilterAsync(ISpecification<Event> specification, CancellationToken cancellationToken = default);
    Task<bool> IsSameEventExists(string title, DateTime dateTime, string placeName, CancellationToken cancellationToken = default);
    
}
