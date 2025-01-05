using EventsWebApplication.Application.Specifications;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Abstractions.Specification;
using EventsWebApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Repository;

public class EventsRepository : AppRepository<Event>, IEventsRepository
{
    public EventsRepository(ApplicationDbContext context) : base(context) { }

    //public async Task<IEnumerable<Event>> GetByFilterAsync(DateTime? dateStart, DateTime? dateEnd, string? placeName, string? categoryName, int offset = 1, int limit = 10, CancellationToken cancellationToken = default)
    //{
    //    IQueryable<Event> query = _entities.AsQueryable().AsNoTracking();

    //    if (dateStart != null && dateEnd != null)
    //    {
    //        query = query.Where(e =>
    //            new DateTime(dateStart.Value.Ticks, DateTimeKind.Utc) <= e.EventDateTime.Date &&
    //            e.EventDateTime.Date <= new DateTime(dateEnd.Value.AddDays(1).Ticks, DateTimeKind.Utc));
    //    }

    //    if (!string.IsNullOrEmpty(placeName))
    //    {
    //        query = query.Where(e => e.Place.Name.ToLower().Contains(placeName.ToLower()));
    //    }

    //    if (!string.IsNullOrEmpty(categoryName))
    //    {
    //        query = query.Where(e => e.Category != null && e.Category.Name.ToLower().Contains(categoryName.ToLower()));
    //    }

    //    return await query.OrderBy(e => e.Id).Skip(offset).Take(limit).ToListAsync(cancellationToken);
    //}

    public async Task<IEnumerable<Event>> GetByFilterAsync(ISpecification<Event> specification, CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator<Event>.GetQuery(_entities, specification).ToListAsync(cancellationToken);
    }

    public async Task<bool> IsSameEventExists(string title, DateTime dateTime, string placeName, CancellationToken cancellationToken = default)
    {
        return await _context.Events.AnyAsync(e => e.Title == title && e.EventDateTime == dateTime && e.Place.Name == placeName, cancellationToken);
    }
}
