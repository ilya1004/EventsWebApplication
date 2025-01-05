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
        var cancellationToken = CancellationToken.None;
        var dateStart = DateTime.UtcNow.Date;
        var dateEnd = DateTime.UtcNow.Date.AddDays(7);

        var events = new List<Event>
        {
            new Event
            {
                Id = 1,
                Title = "Event 1",
                Description = null,
                EventDateTime = DateTime.UtcNow.AddDays(1),
                ParticipantsMaxCount = 10,
                Image = null,
                Place = new Place("Place 1", "PLACE 1"),
                Category = null
            },
            new Event
            {
                Id = 2,
                Title = "Event 2",
                Description = null,
                EventDateTime = DateTime.UtcNow.AddDays(3),
                ParticipantsMaxCount = 20,
                Image = null,
                Place = new Place("Place 2", "PLACE 2"),
                Category = null
            }
        };

        var query = new GetEventsByDateRangeQuery(dateStart, dateEnd, 1, 10);

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.PaginatedListAsync(It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken))
            .ReturnsAsync(events);

        var result = await _handler.Handle(query, cancellationToken);

        result.Should().BeEquivalentTo(events);

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.PaginatedListAsync(It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoEventsInDateRange()
    {
        var cancellationToken = CancellationToken.None;
        var dateStart = DateTime.UtcNow.Date;
        var dateEnd = DateTime.UtcNow.Date.AddDays(7);

        var query = new GetEventsByDateRangeQuery(dateStart, dateEnd, 1, 10);

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.PaginatedListAsync(It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken))
            .ReturnsAsync(new List<Event>());

        var result = await _handler.Handle(query, cancellationToken);

        result.Should().BeEmpty();

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.PaginatedListAsync(It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken),
            Times.Once);
    }
}
