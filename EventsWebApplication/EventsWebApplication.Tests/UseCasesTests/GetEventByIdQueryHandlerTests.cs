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
        var eventId = 1;
        var cancellationToken = CancellationToken.None;

        var expectedEvent = new Event
        {
            Id = eventId,
            Title = "Event 1",
            Description = null,
            EventDateTime = DateTime.Now,
            ParticipantsMaxCount = 10,
            Image = null,
            Place = new Place("Place 1", "PLACE 1"),
            Category = null
        };

        var query = new GetEventByIdQuery(eventId, null);

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken, null))
            .ReturnsAsync(expectedEvent);

        var result = await _handler.Handle(query, cancellationToken);

        result.Should().BeEquivalentTo(expectedEvent);

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken, null),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEventWithParticipants_IfProvided()
    {
        var eventId = 1;
        var cancellationToken = CancellationToken.None;

        var expectedEvent = new Event
        {
            Id = eventId,
            Title = "Test Event",
            Description = "Description",
            EventDateTime = DateTime.Now,
            ParticipantsMaxCount = 10,
            Image = null,
            Place = new Place("Place 1", "PLACE 1"),
            Category = null
        };

        Expression<Func<Event, object>>[] includeProperties = [e => e.Participants];

        var query = new GetEventByIdQuery(eventId, includeProperties);

        _unitOfWorkMock.Setup(u => u.EventsRepository
            .GetByIdAsync(eventId, cancellationToken, includeProperties))
            .ReturnsAsync(expectedEvent);

        var result = await _handler.Handle(query, cancellationToken);

        result.Should().BeEquivalentTo(expectedEvent);

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken, includeProperties),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEventNotFound()
    {
        var eventId = 1;
        var cancellationToken = CancellationToken.None;

        var query = new GetEventByIdQuery(eventId, null);

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken, null))
            .ReturnsAsync((Event?)null);

        var act = async () => await _handler.Handle(query, cancellationToken);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Event with ID {eventId} not found.");

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken, null),
            Times.Once);
    }
}
