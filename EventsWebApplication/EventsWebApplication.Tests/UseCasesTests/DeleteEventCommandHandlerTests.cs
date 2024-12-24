using AutoMapper;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeleteEvent;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Entities.Events;
using FluentAssertions;
using Moq;

namespace EventsWebApplication.Tests.UseCasesTests;

public class DeleteEventCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBlobService> _blobServiceMock;
    private readonly DeleteEventCommandHandler _handler;

    public DeleteEventCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _blobServiceMock = new Mock<IBlobService>();
        _handler = new DeleteEventCommandHandler(_unitOfWorkMock.Object, _blobServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteEvent_WhenEventExists()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var eventId = 1;

        var eventObj = new Event("Event 1", null, DateTime.UtcNow, 10, null, new Place("Place 1", "PLACE 1"), null);

        _unitOfWorkMock
            .Setup(u => u.EventsRepository.GetByIdAsync(eventId, cancellationToken))
            .ReturnsAsync(eventObj);

        _unitOfWorkMock
            .Setup(u => u.EventsRepository.DeleteAsync(eventObj, cancellationToken))
            .Returns(Task.CompletedTask);

        var command = new DeleteEventCommand(eventId);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken), Times.Once);

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.DeleteAsync(eventObj, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEventDoesNotExist()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var eventId = 1;

        _unitOfWorkMock
            .Setup(u => u.EventsRepository.GetByIdAsync(eventId, cancellationToken))
            .ReturnsAsync((Event?)null);

        var command = new DeleteEventCommand(eventId);

        // Act
        var act = async () => await _handler.Handle(command, cancellationToken);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Event with ID {eventId} not found.");

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken), Times.Once);

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.DeleteAsync(It.IsAny<Event>(), cancellationToken), Times.Never);
    }
}
