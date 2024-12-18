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
        // Arrange
        var cancellationToken = CancellationToken.None;

        var events = new List<Event>
        {
            new Event("Event 1", null, DateTime.UtcNow.AddDays(1), 100, null, new Place("Place 1", "PLACE 1"), null) { Id = 1 },
            new Event("Event 2", null, DateTime.UtcNow.AddDays(2), 100, null, new Place("Place 2", "PLACE 2"), null) { Id = 2 }
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

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should()
            .BeEquivalentTo(
            [
                new EventWithRemainingPlacesDTO(1, "Event 1", null, events[0].EventDateTime, 100, 30, "Place 1", null),
                new EventWithRemainingPlacesDTO(2, "Event 2", null, events[1].EventDateTime, 100, 20, "Place 2", null)
            ]);

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
        // Arrange
        var cancellationToken = CancellationToken.None;

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.PaginatedListAllAsync(0, 10, cancellationToken))
            .ReturnsAsync([]);

        _unitOfWorkMock.Setup(u =>
            u.ParticipantsRepository.CountParticipantsByEvents(cancellationToken))
            .ReturnsAsync([]);

        var query = new GetEventsWithRemainingPlacesQuery();

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
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
        // Arrange
        var cancellationToken = CancellationToken.None;

        var events = new List<Event>
        {
            new Event("Event 1", null, DateTime.UtcNow.AddDays(1), 100, null, new Place("Place 1", "PLACE 1"), null) { Id = 1 }
        };

        var query = new GetEventsWithRemainingPlacesQuery { PageNo = 1, PageSize = 10 };

        _unitOfWorkMock.Setup(u => u.EventsRepository.PaginatedListAllAsync(0, 10, cancellationToken))
            .ReturnsAsync(events);

        _unitOfWorkMock.Setup(u => u.ParticipantsRepository.CountParticipantsByEvents(cancellationToken))
            .ReturnsAsync([]);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(
            [
                new EventWithRemainingPlacesDTO(1, "Event 1", null, events[0].EventDateTime, 100, 0, "Place 1", null)
            ]);

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.PaginatedListAllAsync(0, 10, cancellationToken), 
            Times.Once);

        _unitOfWorkMock.Verify(u => 
            u.ParticipantsRepository.CountParticipantsByEvents(cancellationToken), 
            Times.Once);
    }
}
