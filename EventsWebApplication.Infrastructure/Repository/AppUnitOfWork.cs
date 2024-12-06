using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Infrastructure.Data;

namespace EventsWebApplication.Infrastructure.Repository;

internal class AppUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Lazy<IEventsRepository> _eventsRepository;
    private readonly Lazy<IRepository<Participant>> _participantsRepository;
    public AppUnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _eventsRepository = new Lazy<IEventsRepository>(() => new EventsRepository(context));
        _participantsRepository = new Lazy<IRepository<Participant>>(() => new AppRepository<Participant>(context));
    }

    public IEventsRepository EventsRepository => _eventsRepository.Value;
    public IRepository<Participant> ParticipantsRepository => _participantsRepository.Value;

    public async Task CreateDatabaseAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task SaveAllAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
