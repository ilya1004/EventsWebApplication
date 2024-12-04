using EventsWebApplication.Infrastructure.Data;

namespace EventsWebApplication.Infrastructure.Repository;

internal class AppUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Lazy<IRepository<Event>> _eventsRepository;
    private readonly Lazy<IRepository<Participant>> _participantsRepository;
    public AppUnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _eventsRepository = new Lazy<IRepository<Event>>(() => new AppRepository<Event>(context));
        _participantsRepository = new Lazy<IRepository<Participant>>(() => new AppRepository<Participant>(context));
    }

    public IRepository<Event> EventsRepository => _eventsRepository.Value;
    public IRepository<Participant> ParticipantsRepository => _participantsRepository.Value;

    public async Task CreateDatabaseAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task SaveAllAsync()
    {
        await _context.SaveChangesAsync();
    }
}
