using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Entities.Events;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace EventsWebApplication.Tests.UseCasesTests;

public class GetEventByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetEventByIdQueryHandler _handler;

    public GetEventByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetEventByIdQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnEvent_WhenEventExists()
    {
        // Arrange
        var eventId = 1;
        var cancellationToken = CancellationToken.None;

        var expectedEvent = new Event("Event 1", null, DateTime.Now, 10, null, new Place("Place 1", "PLACE 1"), null);

        var query = new GetEventByIdQuery(eventId, null);

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken, null))
            .ReturnsAsync(expectedEvent);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedEvent);

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken, null), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEventWithParticipants_IfProvided()
    {
        // Arrange
        var eventId = 1;
        var cancellationToken = CancellationToken.None;

        var expectedEvent = new Event("Test Event", "Description", DateTime.Now, 10, null, new Place("Place 1", "PLACE_1"), null);

        Expression<Func<Event, object>>[] includeProperties = [e => e.Participants];

        var query = new GetEventByIdQuery(eventId, includeProperties);

        _unitOfWorkMock.Setup(u => u.EventsRepository
            .GetByIdAsync(eventId, cancellationToken, includeProperties))
            .ReturnsAsync(expectedEvent);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedEvent);

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken, includeProperties),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEventNotFound()
    {
        // Arrange
        var eventId = 1;
        var cancellationToken = CancellationToken.None;

        var query = new GetEventByIdQuery(eventId, null);

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken, null))
            .ReturnsAsync((Event?)null);

        // Act
        var act = async () => await _handler.Handle(query, cancellationToken);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Event with ID {eventId} not found.");

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken, null), 
            Times.Once);
    }
}
