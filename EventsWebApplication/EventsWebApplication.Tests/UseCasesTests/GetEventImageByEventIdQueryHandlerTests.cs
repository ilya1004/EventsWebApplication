﻿using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventImageByEventId;
using EventsWebApplication.Domain.Abstractions.BlobStorage;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Entities.Events;
using FluentAssertions;
using Moq;

namespace EventsWebApplication.Tests.UseCasesTests;

public class GetEventImageByEventIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBlobService> _blobServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetEventImageByEventIdQueryHandler _handler;

    public GetEventImageByEventIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _blobServiceMock = new Mock<IBlobService>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetEventImageByEventIdQueryHandler(_unitOfWorkMock.Object, _blobServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnImage_WhenEventAndImageExist()
    {
        var eventId = 1;
        var cancellationToken = CancellationToken.None;
        var stream = new Mock<Stream>();
        var imageId = Guid.NewGuid();
        var fileResponse = new FileResponse(stream.Object, "");
        var fileResponseDTO = new FileResponseDTO(stream.Object, "");

        var eventObj = new Event
        {
            Id = eventId,
            Title = "Event 1",
            Description = null,
            EventDateTime = DateTime.Now,
            ParticipantsMaxCount = 10,
            Image = imageId.ToString(),
            Place = new Place("Place 1", "PLACE 1"),
            Category = null
        };

        var query = new GetEventImageByEventIdQuery(eventId);

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken))
            .ReturnsAsync(eventObj);

        _blobServiceMock.Setup(b =>
            b.DownloadAsync(imageId, cancellationToken))
            .ReturnsAsync(fileResponse);

        _mapperMock.Setup(m =>
            m.Map<FileResponseDTO>(fileResponse))
            .Returns(fileResponseDTO);

        var result = await _handler.Handle(query, cancellationToken);

        result.Should()
            .BeEquivalentTo(fileResponseDTO);

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken),
            Times.Once);

        _blobServiceMock.Verify(b =>
            b.DownloadAsync(imageId, cancellationToken),
            Times.Once);

        _mapperMock.Verify(m =>
            m.Map<FileResponseDTO>(fileResponse),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEventHasNoImage()
    {
        var eventId = 1;
        var cancellationToken = CancellationToken.None;

        var eventObj = new Event
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

        var query = new GetEventImageByEventIdQuery(eventId);

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken))
            .ReturnsAsync(eventObj);

        var act = async () => await _handler.Handle(query, cancellationToken);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Event with ID {eventId} don't have an image");

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken),
            Times.Once);

        _blobServiceMock.Verify(b =>
            b.DownloadAsync(It.IsAny<Guid>(), cancellationToken),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenImageIdIsInvalid()
    {
        var eventId = 1;
        var cancellationToken = CancellationToken.None;

        var eventObj = new Event
        {
            Id = eventId,
            Title = "Event 1",
            Description = null,
            EventDateTime = DateTime.Now,
            ParticipantsMaxCount = 10,
            Image = "qwe123",
            Place = new Place("Place 1", "PLACE 1"),
            Category = null
        };

        var query = new GetEventImageByEventIdQuery(eventId);

        _unitOfWorkMock.Setup(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken))
            .ReturnsAsync(eventObj);

        var act = async () => await _handler.Handle(query, cancellationToken);

        await act.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage($"Event with ID {eventId} have an incorrect format of the image name");

        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.GetByIdAsync(eventId, cancellationToken),
            Times.Once);

        _blobServiceMock.Verify(b =>
            b.DownloadAsync(It.IsAny<Guid>(), cancellationToken),
            Times.Never);
    }
}
