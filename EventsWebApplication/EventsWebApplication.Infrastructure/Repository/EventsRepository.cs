using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Repository;

internal class EventsRepository : AppRepository<Event>, IEventsRepository
{
    public EventsRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Event>> GetByFilterAsync(DateTime? dateStart, DateTime? dateEnd, string? placeName, string? categoryName, int offset = 1, int limit = 10, CancellationToken cancellationToken = default)
    {
        IQueryable<Event> query = _entities.AsQueryable().AsNoTracking();

        if (dateStart != null && dateEnd != null)
        {
            query = query.Where(e => dateStart.Value.Date <= e.EventDateTime.Date && e.EventDateTime.Date <= dateEnd.Value.Date);
        }

        if (!string.IsNullOrEmpty(placeName))
        {
            query = query.Where(e => e.Place.Name.ToLower().Contains(placeName.ToLower()));
        }

        if (!string.IsNullOrEmpty(categoryName))
        {
            query = query.Where(e => e.Category != null && e.Category.Name.ToLower().Contains(categoryName.ToLower()));
        }

        return await query.Skip(offset).Take(limit).ToListAsync(cancellationToken);
    }

    public async Task<bool> IsSameEventExists(string title, DateTime dateTime, string placeName, CancellationToken cancellationToken = default)
    {
        return await _context.Events.AnyAsync(e => e.Title == title && e.EventDateTime == dateTime && e.Place.Name == placeName, cancellationToken);
    }
}
