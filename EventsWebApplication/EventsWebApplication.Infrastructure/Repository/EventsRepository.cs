using EventsWebApplication.Application.Specifications;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Abstractions.Specification;
using EventsWebApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Repository;

public class EventsRepository : AppRepository<Event>, IEventsRepository
{
    public EventsRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<Event>> GetByFilterAsync(ISpecification<Event> specification, CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator<Event>.GetQuery(_entities.AsQueryable(), specification);
        
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> IsSameEventExists(string title, DateTime dateTime, string placeName, CancellationToken cancellationToken = default)
    {
        return await _context.Events.AnyAsync(e => e.Title == title && e.EventDateTime == dateTime && e.Place.Name == placeName, cancellationToken);
    }
}
