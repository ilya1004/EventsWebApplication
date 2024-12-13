using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Repository;

internal class ParticipantsRepository : AppRepository<Participant>, IParticipantsRepository
{
    public ParticipantsRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<(int EventId, int Count)>> CountParticipantsByEvents(CancellationToken cancellationToken = default)
    {
        var result = await _context.Participants
            .GroupBy(p => p.EventId)
            .Select(g => new { EventId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        return result.Select(r => (r.EventId, r.Count));
    }
}
