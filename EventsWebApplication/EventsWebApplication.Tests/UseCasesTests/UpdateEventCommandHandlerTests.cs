using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Domain.Entities.Participants;
using FluentAssertions;
using FluentEmail.Core;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;
using System.Threading;

namespace EventsWebApplication.Tests.UseCasesTests;

public class UpdateEventCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBlobService> _blobServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IFluentEmail> _fluentEmailMock;
    private readonly UpdateEventCommandHandler _handler;

    public UpdateEventCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _blobServiceMock = new Mock<IBlobService>();
        _mapperMock = new Mock<IMapper>();
        _fluentEmailMock = new Mock<IFluentEmail>();

        _handler = new UpdateEventCommandHandler(
            _unitOfWorkMock.Object,
            _blobServiceMock.Object,
            _mapperMock.Object,
            _fluentEmailMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateEvent_WhenNoImageProvided()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var existingEvent = new Event("Event 1", null, new DateTime(2025, 1, 1), 10, null, new Place("Place 1", "PLACE 1"), null); 
        
        var eventDTO = new EventDTO("Event 2", null, new DateTime(2025, 1, 1), 10, null, "Place 1", null);
        
        var command = new UpdateEventCommand(existingEvent.Id, eventDTO);

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync(existingEvent);

        _mapperMock.Setup(m => 
            m.Map<Event>(eventDTO))
            .Returns(existingEvent);

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.UpdateAsync(It.IsAny<Event>(), cancellationToken))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => 
            u.SaveAllAsync(cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.UpdateAsync(It.IsAny<Event>(), cancellationToken), 
            Times.Once);

        _unitOfWorkMock.Verify(u => 
            u.SaveAllAsync(cancellationToken), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUploadNewImage_AndDeleteOldImage()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var existingEvent = new Event("Event 1", null, new DateTime(2025, 1, 1), 10, Guid.NewGuid().ToString(), new Place("Place 1", "PLACE 1"), null);
        
        var fakeFile = new Mock<IFormFile>();
        
        fakeFile.Setup(f => f.OpenReadStream())
            .Returns(new MemoryStream());
        
        fakeFile.Setup(f => f.ContentType)
            .Returns("image/png");

        var eventDTO = new EventDTO("Event 2", null, new DateTime(2025, 1, 1), 10, fakeFile.Object, "Place 2", null);
        
        var command = new UpdateEventCommand(existingEvent.Id, eventDTO);

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync(existingEvent);
        
        _blobServiceMock.Setup(b => 
            b.DeleteAsync(It.IsAny<Guid>(), cancellationToken))
            .Returns(Task.CompletedTask);
        
        _blobServiceMock.Setup(b => 
            b.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), cancellationToken))
            .ReturnsAsync(Guid.NewGuid());
        
        _mapperMock.Setup(m => 
            m.Map<Event>(eventDTO))
            .Returns(existingEvent);
        
        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.UpdateAsync(It.IsAny<Event>(), cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _blobServiceMock.Verify(b => 
            b.DeleteAsync(It.IsAny<Guid>(), cancellationToken), 
            Times.Once);

        _blobServiceMock.Verify(b => 
            b.UploadAsync(It.IsAny<Stream>(), "image/png", cancellationToken), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldSendEmailNotification_WhenDateOrPlaceChanged()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var existingEvent = new Event("Event 1", null, DateTime.Now, 10, null, new Place("Place 1", "PLACE 1"), null);
        
        var eventDTO = new EventDTO("Event 2", null, DateTime.Now.AddDays(1), 10, null, "Place 2", null);
        
        var command = new UpdateEventCommand(existingEvent.Id, eventDTO);

        var participant = new Participant("test@gmail.com", new Person("Name 1", "Surname 1", DateTime.Now), existingEvent);

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync(existingEvent);
        
        _unitOfWorkMock.Setup(u => 
            u.ParticipantsRepository.ListAsync(It.IsAny<Expression<Func<Participant, bool>>>(), cancellationToken))
            .ReturnsAsync([participant]);

        _fluentEmailMock.Setup(e => e.To(It.IsAny<string>())).Returns(_fluentEmailMock.Object);
        _fluentEmailMock.Setup(e => e.Subject(It.IsAny<string>())).Returns(_fluentEmailMock.Object);
        _fluentEmailMock.Setup(e => e.Body(It.IsAny<string>(), It.IsAny<bool>())).Returns(_fluentEmailMock.Object);

        _fluentEmailMock.Setup(e => 
            e.SendAsync(cancellationToken))
            .Returns(Task.FromResult(new FluentEmail.Core.Models.SendResponse()));

        _mapperMock.Setup(m => m.Map<Event>(eventDTO)).Returns(existingEvent);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _fluentEmailMock.Verify(e => e.To(participant.Email), 
            Times.Once);

        _fluentEmailMock.Verify(e => e.Subject(It.IsAny<string>()), 
            Times.Once);

        _fluentEmailMock.Verify(e => e.Body(It.Is<string>(body => body.Contains("Place 2")), true), 
            Times.Once);
        
        _fluentEmailMock.Verify(e => e.SendAsync(cancellationToken), 
            Times.Once);
    }
}
