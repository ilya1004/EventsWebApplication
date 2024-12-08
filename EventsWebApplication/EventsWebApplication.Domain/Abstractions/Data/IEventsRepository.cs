using EventsWebApplication.Domain.Entities.Events;

namespace EventsWebApplication.Domain.Abstractions.Data;

public interface IEventsRepository : IRepository<Event>
{
    public Task<bool> IsSameEventExists(string title, DateTime dateTime, string placeName, CancellationToken cancellationToken = default);

    public Task<IEnumerable<Event>> GetByFilterAsync(DateTime? dateStart, DateTime? dateEnd, string? placeName, string? categoryName, int offset = 1, int limit = 10, CancellationToken cancellationToken = default);
}
