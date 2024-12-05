using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Repository;

internal class EventsRepository : AppRepository<Event>, IEventsRepository
{
    public EventsRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> IsSameEventExists(string title, DateTime dateTime, string placeName)
    {
        return await _context.Events.AnyAsync(e => e.Title == title && e.EventDateTime == dateTime && e.Place.Name == placeName);
    }
}
