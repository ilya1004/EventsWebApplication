using EventsWebApplication.Domain.Entities.Participants;

namespace EventsWebApplication.Domain.Abstractions.Data;

public interface IParticipantsRepository : IRepository<Participant>
{
    public Task<IEnumerable<(int EventId, int Count)>> CountParticipantsByEvents(CancellationToken cancellationToken = default)
}
