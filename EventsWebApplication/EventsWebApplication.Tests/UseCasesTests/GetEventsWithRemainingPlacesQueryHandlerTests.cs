using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsWithRemainingPlaces;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Entities.Events;
using FluentAssertions;
using Moq;

namespace EventsWebApplication.Tests.UseCasesTests;

public class GetEventsWithRemainingPlacesQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetEventsWithRemainingPlacesQueryHandler _handler;

    public GetEventsWithRemainingPlacesQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetEventsWithRemainingPlacesQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnEventsWithRemainingPlaces_WhenParticipantsAndEventsExist()
    {
        var cancellationToken = CancellationToken.None;

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
                Category = new Category("Category 1", "CATEGORY 1"),
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
                Category = new Category("Category 2", "CATEGORY 2"),
            }
        };

        var participantCounts = new List<(int EventId, int Count)>
        {
            (1, 30),
            (2, 20)
        };

        var query = new GetEventsWithRemainingPlacesQuery();

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.PaginatedListAllAsync(0, 10, cancellationToken))
            .ReturnsAsync(events);

        _unitOfWorkMock.Setup(u =>
            u.ParticipantsRepository.CountParticipantsByEvents(cancellationToken))
            .ReturnsAsync(participantCounts);

        var result = await _handler.Handle(query, cancellationToken);

        result.Should()
            .BeEquivalentTo(
                new List<EventWithRemainingPlacesDTO>
                {
                    new EventWithRemainingPlacesDTO(1, "Event 1", null, events[0].EventDateTime, 100, 30, "Place 1", "Category 1"),
                    new EventWithRemainingPlacesDTO(2, "Event 2", null, events[1].EventDateTime, 100, 20, "Place 2", "Category 2")
                });

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.PaginatedListAllAsync(0, 10, cancellationToken),
            Times.Once);

        _unitOfWorkMock.Verify(u =>
            u.ParticipantsRepository.CountParticipantsByEvents(cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoEventsExist()
    {
        var cancellationToken = CancellationToken.None;

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.PaginatedListAllAsync(0, 10, cancellationToken))
            .ReturnsAsync(new List<Event>());

        _unitOfWorkMock.Setup(u =>
            u.ParticipantsRepository.CountParticipantsByEvents(cancellationToken))
            .ReturnsAsync(new List<(int EventId, int Count)>());

        var query = new GetEventsWithRemainingPlacesQuery();

        var result = await _handler.Handle(query, cancellationToken);

        result.Should().BeEmpty();

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.PaginatedListAllAsync(0, 10, cancellationToken),
            Times.Once);

        _unitOfWorkMock.Verify(u =>
            u.ParticipantsRepository.CountParticipantsByEvents(cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldHandleEventsWithNoParticipants()
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
                ParticipantsMaxCount = 100,
                Image = null,
                Place = new Place("Place 1", "PLACE 1"),
                Category = new Category("Category 1", "CATEGORY 1"),
            }
        };

        var query = new GetEventsWithRemainingPlacesQuery();

        _unitOfWorkMock.Setup(u => u.EventsRepository.PaginatedListAllAsync(0, 10, cancellationToken))
            .ReturnsAsync(events);

        _unitOfWorkMock.Setup(u => u.ParticipantsRepository.CountParticipantsByEvents(cancellationToken))
            .ReturnsAsync(new List<(int EventId, int Count)>());

        var result = await _handler.Handle(query, cancellationToken);

        result.Should().BeEquivalentTo(
            new List<EventWithRemainingPlacesDTO>
            {
                new EventWithRemainingPlacesDTO(1, "Event 1", "Description 1", events[0].EventDateTime, 100, 0, "Place 1", "Category 1")
            });

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.PaginatedListAllAsync(0, 10, cancellationToken),
            Times.Once);

        _unitOfWorkMock.Verify(u =>
            u.ParticipantsRepository.CountParticipantsByEvents(cancellationToken),
            Times.Once);
    }
}

