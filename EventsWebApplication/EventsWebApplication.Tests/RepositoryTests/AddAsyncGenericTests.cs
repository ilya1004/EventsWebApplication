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

        var eventEntity = new Event(
            "Event 1",
            "Description 1",
            DateTime.UtcNow.AddDays(1),
            100,
            null,
            new Place("Place 1", "PLACE 1"),
            new Category("Category 1", "CATEGORY 1"))
        {
            Id = 1
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
        context.SaveChanges();
        var repository = new AppRepository<Event>(context);

        var eventEntity = new Event(
            "Event 1",
            "Description 1",
            DateTime.UtcNow,
            50,
            null,
            new Place("Place 1", "PLACE 1"),
            null)
            {
                Id = 2
            };

        await repository.AddAsync(eventEntity, cancellationToken);

        var count = await context.Events.CountAsync();
        count.Should().Be(0);
    }
}
