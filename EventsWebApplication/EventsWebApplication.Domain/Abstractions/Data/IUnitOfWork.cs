using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Domain.Entities.Participants;

namespace EventsWebApplication.Domain.Abstractions.Data;

public interface IUnitOfWork
{
    IEventsRepository EventsRepository { get; }
    IRepository<Participant> ParticipantsRepository { get; }

    public Task SaveAllAsync(CancellationToken cancellationToken);
    public Task CreateDatabaseAsync();
}
