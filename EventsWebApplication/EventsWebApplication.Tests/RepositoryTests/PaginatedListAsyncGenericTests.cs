using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventsWebApplication.Tests.RepositoryTests;

public class PaginatedListAsyncGenericTests
{
    private async Task<ApplicationDbContext> GetInMemoryDbContextAsync()
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
        // Arrange
        var context = await GetInMemoryDbContextAsync();
        context.Events.RemoveRange(context.Events);
        context.SaveChanges();
        var repository = new AppRepository<Event>(context);

        var events = new List<Event>
        {
            new Event("Event 1", null, DateTime.UtcNow.AddDays(1), 100, null, new Place("Place 1", "PLACE 1"), null) { Id = 1 },
            new Event("Event 2", null, DateTime.UtcNow.AddDays(2), 100, null, new Place("Place 2", "PLACE 2"), null) { Id = 2 },
            new Event("Event 3", null, DateTime.UtcNow.AddDays(3), 100, null, new Place("Place 1", "PLACE 1"), null) { Id = 3 },
            new Event("Event 4", null, DateTime.UtcNow.AddDays(4), 100, null, new Place("Place 4", "PLACE 4"), null) { Id = 4 }
        };

        await context.Events.AddRangeAsync(events);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.PaginatedListAsync(e => e.Place.Name == "Place 1", 0, 2, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Title.Should().Be("Event 1");
        result[1].Title.Should().Be("Event 3");
    }

    [Fact]
    public async Task PaginatedListAsync_ShouldReturnEmptyList_WhenNoDataMatchesFilter()
    {
        // Arrange
        var context = await GetInMemoryDbContextAsync();
        context.Events.RemoveRange(context.Events);
        context.SaveChanges();
        var repository = new AppRepository<Event>(context);

        var events = new List<Event>
        {
            new Event("Event 1", "Description 1", DateTime.UtcNow.AddDays(1), 100, null, new Place("Place 1", "PLACE 1"), null) { Id = 1 },
            new Event("Event 2", "Description 2", DateTime.UtcNow.AddDays(2), 100, null, new Place("Place 2", "PLACE 2"), null) { Id = 2 }
        };

        await context.Events.AddRangeAsync(events);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.PaginatedListAsync(e => e.Place.Name == "Place 123", 0, 2, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task PaginatedListAsync_ShouldReturnCorrectPageOfData()
    {
        // Arrange
        var context = await GetInMemoryDbContextAsync();
        context.Events.RemoveRange(context.Events);
        context.SaveChanges();
        var repository = new AppRepository<Event>(context);

        var events = new List<Event>
        {
            new Event("Event 1", null, DateTime.UtcNow.AddDays(1), 100, null, new Place("Place 1", "PLACE 1"), null) { Id = 1 },
            new Event("Event 2", null, DateTime.UtcNow.AddDays(2), 100, null, new Place("Place 2", "PLACE 2"), null) { Id = 2 },
            new Event("Event 3", null, DateTime.UtcNow.AddDays(3), 100, null, new Place("Place 3", "PLACE 3"), null) { Id = 3 },
            new Event("Event 4", null, DateTime.UtcNow.AddDays(4), 100, null, new Place("Place 4", "PLACE 4"), null) { Id = 4 }
        };

        await context.Events.AddRangeAsync(events);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.PaginatedListAsync(null, 2, 2, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Title.Should().Be("Event 3");
        result[1].Title.Should().Be("Event 4");
    }
}
