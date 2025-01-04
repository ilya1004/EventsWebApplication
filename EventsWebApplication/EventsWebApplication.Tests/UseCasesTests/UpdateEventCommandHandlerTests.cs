using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Abstractions.EmailSenderService;
using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Domain.Entities.Participants;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;

namespace EventsWebApplication.Tests.UseCasesTests;

public class UpdateEventCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBlobService> _blobServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IEmailSenderService> _emailSenderServiceMock;
    private readonly UpdateEventCommandHandler _handler;

    public UpdateEventCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _blobServiceMock = new Mock<IBlobService>();
        _mapperMock = new Mock<IMapper>();
        _emailSenderServiceMock = new Mock<IEmailSenderService>();

       _handler = new UpdateEventCommandHandler(
            _unitOfWorkMock.Object,
            _blobServiceMock.Object,
            _mapperMock.Object,
            _emailSenderServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateEvent_WhenNoImageProvided()
    {
        var cancellationToken = CancellationToken.None;
        var existingEvent = new Event("Event 1", null, new DateTime(2025, 1, 1), 10, null, new Place("Place 1", "PLACE 1"), null) { Id = 1 };
        var eventDTO = new EventDTO("Event 2", null, new DateTime(2025, 1, 1), 10, "Place 1", null);
        var command = new UpdateEventCommand(existingEvent.Id, eventDTO, null, null);

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync(existingEvent);

        _mapperMock.Setup(m => 
            m.Map<Event>(command))
            .Returns(existingEvent);

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.UpdateAsync(It.IsAny<Event>(), cancellationToken))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => 
            u.SaveAllAsync(cancellationToken))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

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
        var cancellationToken = CancellationToken.None;
        var existingEvent = new Event("Event 1", null, new DateTime(2025, 1, 1), 10, Guid.NewGuid().ToString(), new Place("Place 1", "PLACE 1"), null) { Id = 1 };
        var fakeFile = new Mock<IFormFile>();
        
        fakeFile.Setup(f => f.OpenReadStream())
            .Returns(new MemoryStream());
        
        fakeFile.Setup(f => f.ContentType)
            .Returns("image/png");

        var eventDTO = new EventDTO("Event 2", null, new DateTime(2025, 1, 1), 10, "Place 2", null);
        
        var command = new UpdateEventCommand(existingEvent.Id, eventDTO, fakeFile.Object.OpenReadStream(), fakeFile.Object.ContentType);

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
            m.Map<Event>(command))
            .Returns(existingEvent);
        
        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.UpdateAsync(It.IsAny<Event>(), cancellationToken))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

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
        var cancellationToken = CancellationToken.None;

        var existingEvent = new Event("Event 1", null, DateTime.Now, 10, null, new Place("Place 1", "PLACE 1"), null) { Id = 1 };
        var newEventDTO = new EventDTO("Event 2", null, DateTime.Now.AddDays(1), 10, "Place 2", null);
        var newEvent = new Event("Event 2", null, DateTime.Now.AddDays(1), 10, null, new Place("Place 2", "PLACE 2"), null) { Id = 1 };

        var command = new UpdateEventCommand(existingEvent.Id, newEventDTO, null, null);

        var participant = new Participant
        {
            Id = 1,
            Email = "test@gmail.com",
            Person = new Person("Name 1", "Surname 1", DateTime.Now),
            Event = existingEvent,
            EventId = existingEvent.Id,
            EventRegistrationDate = DateTime.UtcNow,
        };

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync(existingEvent);

        _unitOfWorkMock.Setup(u =>
            u.ParticipantsRepository.ListAsync(It.IsAny<Expression<Func<Participant, bool>>>(), cancellationToken))
            .ReturnsAsync(new List<Participant> { participant });

        _mapperMock.Setup(m => m.Map<Event>(command))
            .Returns(newEvent);


        _emailSenderServiceMock.Setup(es => es.SendEmailNotifications(It.IsAny<Event>(), It.IsAny<Event>(), cancellationToken))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var handler = new UpdateEventCommandHandler(
            _unitOfWorkMock.Object,
            _blobServiceMock.Object,
            _mapperMock.Object,
            _emailSenderServiceMock.Object);

        await handler.Handle(command, cancellationToken);

        _unitOfWorkMock.Verify(u => u.EventsRepository.GetByIdAsync(command.Id, cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(u => u.EventsRepository.UpdateAsync(It.IsAny<Event>(), cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAllAsync(cancellationToken), Times.Once);

        _emailSenderServiceMock.Verify(es => es.SendEmailNotifications(
            It.IsAny<Event>(),
            It.IsAny<Event>(),
            cancellationToken),
            Times.Once);
    }
}
