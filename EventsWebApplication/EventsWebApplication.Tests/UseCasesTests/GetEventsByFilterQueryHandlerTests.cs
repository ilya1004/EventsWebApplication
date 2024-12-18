using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByFilter;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Entities.Events;
using FluentAssertions;
using Moq;

namespace EventsWebApplication.Tests.UseCasesTests;

public class GetEventsByFilterQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetEventsByFilterQueryHandler _handler;

    public GetEventsByFilterQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetEventsByFilterQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFilteredEvents_WhenEventsMatchFilters()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var events = new List<Event>
        {
            new Event("Event 1", "Description 1", DateTime.UtcNow.AddDays(1), 10, null, 
                new Place("Place 1", "PLACE 1"), new Category("Category 1", "CATEGORY 1")),

            new Event("Event 2", "Description 2", DateTime.UtcNow.AddDays(3), 10, null,
                new Place("Place 1", "PLACE 1"), new Category("Category 1", "CATEGORY 1"))
        };

        var query = new GetEventsByFilterQuery(
            DateTime.UtcNow.AddDays(-7),
            DateTime.UtcNow.AddDays(7),
            "Place 1",
            "Category 1");

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.GetByFilterAsync(
                query.DateStart, query.DateEnd, query.PlaceName, query.CategoryName, 0, 10, cancellationToken))
            .ReturnsAsync(events);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(events);

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.GetByFilterAsync(
                query.DateStart, query.DateEnd, query.PlaceName, query.CategoryName, 0, 10, cancellationToken), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoEventsMatchFilters()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var query = new GetEventsByFilterQuery(
            DateTime.UtcNow.AddDays(-7),
            DateTime.UtcNow.AddDays(7),
            "Place 1",
            "Category 1");

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.GetByFilterAsync(
                query.DateStart, query.DateEnd, query.PlaceName, query.CategoryName, 0, 10, cancellationToken))
            .ReturnsAsync(new List<Event>());

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().BeEmpty();

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.GetByFilterAsync(
                query.DateStart, query.DateEnd, query.PlaceName, query.CategoryName, 0, 10, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFullPaginatedList_WhenFiltersIsNull()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var query = new GetEventsByFilterQuery(null, null, null, null);

        var events = new List<Event>
        {
            new Event("Event 1", "Description 1", DateTime.UtcNow.AddDays(1), 10, null, new Place("Place 1", "PLACE_1"), null),
            new Event("Event 2", "Description 2", DateTime.UtcNow.AddDays(3), 20, null, new Place("Place 2", "PLACE_2"), null)
        };

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.GetByFilterAsync(
                query.DateStart, query.DateEnd, query.PlaceName, query.CategoryName, 0, 10, cancellationToken))
            .ReturnsAsync(events);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(events);

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.GetByFilterAsync(
                query.DateStart, query.DateEnd, query.PlaceName, query.CategoryName, 0, 10, cancellationToken), 
            Times.Once);
    }
}
