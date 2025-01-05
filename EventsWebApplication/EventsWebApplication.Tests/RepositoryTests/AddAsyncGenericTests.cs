using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Tests.RepositoryTests;

public class AddAsyncGenericTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("InMemoryDB1")
            .Options;

        var context = new ApplicationDbContext(options);

        return context;
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntityToDatabase()
    {
        var cancellationToken = CancellationToken.None;
        var context = GetInMemoryDbContext();
        context.Events.RemoveRange(context.Events);
        context.SaveChanges();
        var repository = new AppRepository<Event>(context);

        var eventEntity = new Event
        {
            Id = 1,
            Title = "Event 1",
            Description = "Description 1",
            EventDateTime = DateTime.UtcNow.AddDays(1),
            ParticipantsMaxCount = 100,
            Image = null,
            Place = new Place("Place 1", "PLACE 1"),
            Category = new Category("Category 1", "CATEGORY 1"),
        };

        await repository.AddAsync(eventEntity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var addedEntity = await context.Events.FirstOrDefaultAsync(e => e.Id == eventEntity.Id);

        addedEntity.Should().NotBeNull();
        addedEntity!.Title.Should().Be("Event 1");
        addedEntity.Description.Should().Be("Description 1");
        addedEntity.ParticipantsMaxCount.Should().Be(100);
        addedEntity.Place.Name.Should().Be("Place 1");
        addedEntity.Category!.Name.Should().Be("Category 1");
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenEntityIsNull()
    {
        var cancellationToken = CancellationToken.None;
        var context = GetInMemoryDbContext();
        context.Events.RemoveRange(context.Events);
        context.SaveChanges();
        var repository = new AppRepository<Event>(context);

        var act = async () => await repository.AddAsync(null!, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddAsync_ShouldSaveChangesToDatabase()
    {
        var cancellationToken = CancellationToken.None;
        var context = GetInMemoryDbContext();
        context.Events.RemoveRange(context.Events);
        await context.SaveChangesAsync();
        var repository = new AppRepository<Event>(context);

        var eventEntity = new Event
        {
            Id = 2,
            Title = "Event 1",
            Description = "Description 1",
            EventDateTime = DateTime.UtcNow,
            ParticipantsMaxCount = 50,
            Image = null,
            Place = new Place("Place 1", "PLACE 1"),
            Category = null,
        };

        await repository.AddAsync(eventEntity, cancellationToken);
        await context.SaveChangesAsync();

        var count = await context.Events.CountAsync();
        count.Should().Be(1);
    }
}
