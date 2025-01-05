using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Tests.RepositoryTests;

public class PaginatedListAsyncGenericTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("InMemoryDB2")
            .Options;

        var context = new ApplicationDbContext(options);

        return context;
    }

    [Fact]
    public async Task PaginatedListAsync_ShouldReturnFilteredAndPaginatedData()
    {
        var context = GetInMemoryDbContext();
        context.Events.RemoveRange(context.Events);
        context.SaveChanges();
        var repository = new AppRepository<Event>(context);

        var events = new List<Event>
        {
            new Event
            {
                Id = 1,
                Title = "Event 1",
                Description = null,
                EventDateTime = DateTime.UtcNow.AddDays(1),
                ParticipantsMaxCount = 100,
                Image = null,
                Place = new Place("Place 1", "PLACE 1"),
                Category = null,
            },
            new Event
            {
                Id = 2,
                Title = "Event 2",
                Description = null,
                EventDateTime = DateTime.UtcNow.AddDays(2),
                ParticipantsMaxCount = 100,
                Image = null,
                Place = new Place("Place 2", "PLACE 2"),
                Category = null,
            },
            new Event
            {
                Id = 3,
                Title = "Event 3",
                Description = null,
                EventDateTime = DateTime.UtcNow.AddDays(3),
                ParticipantsMaxCount = 100,
                Image = null,
                Place = new Place("Place 1", "PLACE 1"),
                Category = null,
            },
            new Event
            {
                Id = 4,
                Title = "Event 4",
                Description = null,
                EventDateTime = DateTime.UtcNow.AddDays(4),
                ParticipantsMaxCount = 100,
                Image = null,
                Place = new Place("Place 4", "PLACE 4"),
                Category = null,
            }
        };

        await context.Events.AddRangeAsync(events);
        await context.SaveChangesAsync();

        var result = await repository.PaginatedListAsync(e => e.Place.Name == "Place 1", 0, 2, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Title.Should().Be("Event 1");
        result[1].Title.Should().Be("Event 3");
    }

    [Fact]
    public async Task PaginatedListAsync_ShouldReturnEmptyList_WhenNoDataMatchesFilter()
    {
        var context = GetInMemoryDbContext();
        context.Events.RemoveRange(context.Events);
        context.SaveChanges();
        var repository = new AppRepository<Event>(context);

        var events = new List<Event>
        {
            new Event
            {
                Id = 1,
                Title = "Event 1",
                Description = "Description 1",
                EventDateTime = DateTime.UtcNow.AddDays(1),
                ParticipantsMaxCount = 100,
                Image = null,
                Place = new Place("Place 1", "PLACE 1"),
                Category = null,
            },
            new Event
            {
                Id = 2,
                Title = "Event 2",
                Description = "Description 2",
                EventDateTime = DateTime.UtcNow.AddDays(2),
                ParticipantsMaxCount = 100,
                Image = null,
                Place = new Place("Place 2", "PLACE 2"),
                Category = null,
            }
        };

        await context.Events.AddRangeAsync(events);
        await context.SaveChangesAsync();

        var result = await repository.PaginatedListAsync(e => e.Place.Name == "Place 123", 0, 2, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task PaginatedListAsync_ShouldReturnCorrectPageOfData()
    {
        var context = GetInMemoryDbContext();
        context.Events.RemoveRange(context.Events);
        context.SaveChanges();
        var repository = new AppRepository<Event>(context);

        var events = new List<Event>
        {
            new Event
            {
                Id = 1,
                Title = "Event 1",
                Description = null,
                EventDateTime = DateTime.UtcNow.AddDays(1),
                ParticipantsMaxCount = 100,
                Image = null,
                Place = new Place("Place 1", "PLACE 1"),
                Category = null,
            },
            new Event
            {
                Id = 2,
                Title = "Event 2",
                Description = null,
                EventDateTime = DateTime.UtcNow.AddDays(2),
                ParticipantsMaxCount = 100,
                Image = null,
                Place = new Place("Place 2", "PLACE 2"),
                Category = null,
            },
            new Event
            {
                Id = 3,
                Title = "Event 3",
                Description = null,
                EventDateTime = DateTime.UtcNow.AddDays(3),
                ParticipantsMaxCount = 100,
                Image = null,
                Place = new Place("Place 3", "PLACE 3"),
                Category = null,
            },
            new Event
            {
                Id = 4,
                Title = "Event 4",
                Description = null,
                EventDateTime = DateTime.UtcNow.AddDays(4),
                ParticipantsMaxCount = 100,
                Image = null,
                Place = new Place("Place 4", "PLACE 4"),
                Category = null,
            }
        };

        await context.Events.AddRangeAsync(events);
        await context.SaveChangesAsync();

        var result = await repository.PaginatedListAsync(null, 2, 2, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Title.Should().Be("Event 3");
        result[1].Title.Should().Be("Event 4");
    }
}
