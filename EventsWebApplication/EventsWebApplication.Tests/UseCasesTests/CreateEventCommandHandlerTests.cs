using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Entities.Events;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace EventsWebApplication.Tests.UseCasesTests;

public class CreateEventCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IBlobService> _blobServiceMock;
    private readonly CreateEventCommandHandler _handler;

    public CreateEventCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _blobServiceMock = new Mock<IBlobService>();

        _handler = new CreateEventCommandHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _blobServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEventAlreadyExists()
    {
        var eventDTO = new EventDTO(
            "Event 1",
            "",
            DateTime.UtcNow.AddDays(1),
            10,
            "Place 1",
            "Category 1");

        var command = new CreateEventCommand(eventDTO, null, null);
        var cancellationToken = CancellationToken.None;

        _unitOfWorkMock
            .Setup(u => u.EventsRepository.IsSameEventExists(
                eventDTO.Title, eventDTO.EventDateTime, eventDTO.PlaceName, cancellationToken))
            .ReturnsAsync(true);

        var act = async () => await _handler.Handle(command, cancellationToken);

        await act.Should()
            .ThrowAsync<AlreadyExistsException>()
            .WithMessage("Event with this Title, DateTime and Place already exists");

        _unitOfWorkMock.Verify(u => u.EventsRepository.IsSameEventExists(
            eventDTO.Title, eventDTO.EventDateTime, eventDTO.PlaceName, cancellationToken), Times.Once);

        _unitOfWorkMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldCreateEvent_WhenImageIsProvided()
    {
        var eventDTO = new EventDTO(
            "Event 1",
            "",
            DateTime.UtcNow.AddDays(1),
            10,
            "Place 1",
            "Category 1");

        var fakeFormFile = new FakeFormFile();

        var command = new CreateEventCommand(eventDTO, fakeFormFile.OpenReadStream(), fakeFormFile.ContentType);
        var cancellationToken = CancellationToken.None;

        _unitOfWorkMock
            .Setup(u => u.EventsRepository.IsSameEventExists(
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), cancellationToken))
            .ReturnsAsync(false);

        _blobServiceMock
            .Setup(b => b.UploadAsync(
                It.IsAny<Stream>(), It.IsAny<string>(), cancellationToken))
            .ReturnsAsync(Guid.NewGuid());

        var newEventObj = new Event("Event 1", null, eventDTO.EventDateTime, 10, null, new Place("Place 1", "PLACE 1"), null);

        _mapperMock
            .Setup(m => m.Map<Event>(command))
            .Returns(newEventObj);

        _unitOfWorkMock
            .Setup(u => u.EventsRepository.AddAsync(It.IsAny<Event>(), cancellationToken))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveAllAsync(cancellationToken))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, cancellationToken);

        _blobServiceMock.Verify(b => b.UploadAsync(
            It.IsAny<Stream>(), command.ContentType!, cancellationToken), Times.Once);

        _unitOfWorkMock.Verify(u => u.EventsRepository.AddAsync(It.IsAny<Event>(), cancellationToken), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveAllAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateEvent_WhenImageIsNotProvided()
    {
        var eventDTO = new EventDTO(
            "Event 1",
            "",
            DateTime.UtcNow.AddDays(1),
            10,
            "Place 1",
            "Category 1");

        var command = new CreateEventCommand(eventDTO, null, null);
        var cancellationToken = CancellationToken.None;

        _unitOfWorkMock
            .Setup(u => u.EventsRepository.IsSameEventExists(
                It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), cancellationToken))
            .ReturnsAsync(false);

        var newEventObj = new Event("Event 1", null, eventDTO.EventDateTime, 10, null, new Place("Place 1", "PLACE 1"), null);

        _mapperMock
            .Setup(m => m.Map<Event>(command))
            .Returns(newEventObj);

        _unitOfWorkMock
            .Setup(u => u.EventsRepository.AddAsync(It.IsAny<Event>(), cancellationToken))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveAllAsync(cancellationToken))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, cancellationToken);

        _unitOfWorkMock.Verify(u => u.EventsRepository.AddAsync(It.IsAny<Event>(), cancellationToken), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveAllAsync(cancellationToken), Times.Once);
    }

    private class FakeFormFile : IFormFile
    {
        public Stream OpenReadStream() => new MemoryStream([]);
        public string ContentType => "image/png";
        public string FileName => "test.png";
        public string Name => "test";
        public long Length => 0;
        public string ContentDisposition => "";
        public IHeaderDictionary Headers => throw new NotImplementedException();
        public void CopyTo(Stream target) { }
        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}
