using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDateRange;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Entities.Events;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace EventsWebApplication.Tests.UseCasesTests;

public class GetEventsByDateRangeQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetEventsByDateRangeQueryHandler _handler;

    public GetEventsByDateRangeQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetEventsByDateRangeQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnEvents_WhenEventsExistInDateRange()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var dateStart = DateTime.UtcNow.Date;
        var dateEnd = DateTime.UtcNow.Date.AddDays(7);

        var events = new List<Event>
        {
            new Event("Event 1", null, DateTime.UtcNow.AddDays(1), 10, null, new Place("Place 1", "PLACE 1"), null),
            new Event("Event 2", null, DateTime.UtcNow.AddDays(3), 20, null, new Place("Place 2", "PLACE 2"), null)
        };

        var query = new GetEventsByDateRangeQuery(dateStart, dateEnd, 1, 10);

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.PaginatedListAsync(It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken))
            .ReturnsAsync(events);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(events);

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.PaginatedListAsync(It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoEventsInDateRange()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var dateStart = DateTime.UtcNow.Date;
        var dateEnd = DateTime.UtcNow.Date.AddDays(7);

        var query = new GetEventsByDateRangeQuery(dateStart, dateEnd, 1, 10); // PageNo = 1, PageSize = 10

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.PaginatedListAsync( It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken))
            .ReturnsAsync(new List<Event>());

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().BeEmpty();

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.PaginatedListAsync(It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken), 
            Times.Once);
    }

}
