using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Domain.Entities.Participants;
using EventsWebApplication.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }
}
