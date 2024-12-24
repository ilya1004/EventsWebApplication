using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Entities.Participants;
using EventsWebApplication.Infrastructure.Data;

namespace EventsWebApplication.Infrastructure.Repository;

public class AppUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Lazy<IEventsRepository> _eventsRepository;
    private readonly Lazy<IParticipantsRepository> _participantsRepository;
    public AppUnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _eventsRepository = new Lazy<IEventsRepository>(() => new EventsRepository(context));
        _participantsRepository = new Lazy<IParticipantsRepository>(() => new ParticipantsRepository(context));
    }

    public IEventsRepository EventsRepository => _eventsRepository.Value;
    public IParticipantsRepository ParticipantsRepository => _participantsRepository.Value;


    public async Task SaveAllAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
