using EventsWebApplication.Application.Specifications.EventSpecifications;
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
        var cancellationToken = CancellationToken.None;

        var events = new List<Event>
        {
            new Event 
            {
                Id = 1,
                Title = "Event 1",
                Description = "Description 1",
                EventDateTime = DateTime.UtcNow.AddDays(1),
                ParticipantsMaxCount = 10,
                Image = null,
                Place = new Place("Place 1", "PLACE 1"),
                Category = new Category("Category 1", "CATEGORY 1") 
            },
            new Event 
            { 
                Id = 2,
                Title = "Event 2", 
                Description = "Description 2", 
                EventDateTime = DateTime.UtcNow.AddDays(3), 
                ParticipantsMaxCount = 10,
                Image = null, 
                Place = new Place("Place 1", "PLACE 1"), 
                Category = new Category ("Category 1", "CATEGORY 1") 
            },
        };

        var query = new GetEventsByFilterQuery(
            DateTime.UtcNow.AddDays(-7),
            DateTime.UtcNow.AddDays(7),
            "Place 1",
            "Category 1");

        var specification = new EventsListByFilterSpecification(
            query.DateStart,
            query.DateEnd,
            query.PlaceName,
            query.CategoryName,
            0,
            10);

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.GetByFilterAsync(specification, cancellationToken))
            .ReturnsAsync(events);

        var result = await _handler.Handle(query, cancellationToken);

        result.Should().BeEquivalentTo(events);

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.GetByFilterAsync(specification, cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoEventsMatchFilters()
    {
        var cancellationToken = CancellationToken.None;
        var query = new GetEventsByFilterQuery(
            DateTime.UtcNow.AddDays(-7),
            DateTime.UtcNow.AddDays(7),
            "Place 1",
            "Category 1");

        var specification = new EventsListByFilterSpecification(
            query.DateStart,
            query.DateEnd,
            query.PlaceName,
            query.CategoryName,
            0,
            10);

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.GetByFilterAsync(specification, cancellationToken))
            .ReturnsAsync(new List<Event>());

        var result = await _handler.Handle(query, cancellationToken);

        result.Should().BeEmpty();

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.GetByFilterAsync(specification, cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFullPaginatedList_WhenFiltersIsNull()
    {
        var cancellationToken = CancellationToken.None;
        var query = new GetEventsByFilterQuery(null, null, null, null);

        var events = new List<Event>
        {
            new Event
            {
                Id = 1,
                Title = "Event 1",
                Description = "Description 1",
                EventDateTime = DateTime.UtcNow.AddDays(1),
                ParticipantsMaxCount = 10,
                Image = null,
                Place = new Place("Place 1", "PLACE 1"),
                Category = new Category("Category 1", "CATEGORY 1"),
            },
            new Event
            {
                Id = 2,
                Title = "Event 2",
                Description = "Description 2",
                EventDateTime = DateTime.UtcNow.AddDays(3),
                ParticipantsMaxCount = 20,
                Image = null,
                Place = new Place("Place 2", "PLACE 2"),
                Category = new Category("Category 2", "CATEGORY 2"),
            }
        };

        var specification = new EventsListByFilterSpecification(
            query.DateStart,
            query.DateEnd,
            query.PlaceName,
            query.CategoryName,
            0,
            10);

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.GetByFilterAsync(specification, cancellationToken))
            .ReturnsAsync(events);

        var result = await _handler.Handle(query, cancellationToken);

        result.Should().BeEquivalentTo(events);

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.GetByFilterAsync(specification, cancellationToken), 
            Times.Once);
    }
}
