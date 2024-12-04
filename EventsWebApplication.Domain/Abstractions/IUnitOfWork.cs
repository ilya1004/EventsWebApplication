using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Domain.Entities.Perticipants;

namespace EventsWebApplication.Domain.Abstractions;

public interface IUnitOfWork
{
    IRepository<Event> EventsRepository { get; }
    IRepository<Participant> ParticipantsRepository { get; }

    public Task SaveAllAsync();
    public Task CreateDatabaseAsync();
}
