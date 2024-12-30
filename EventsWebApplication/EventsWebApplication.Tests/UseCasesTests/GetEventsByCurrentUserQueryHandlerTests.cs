using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByCurrentUser;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Domain.Entities.Participants;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace EventsWebApplication.Tests.UseCasesTests;

public class GetEventsByCurrentUserQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetEventsByCurrentUserQueryHandler _handler;

    public GetEventsByCurrentUserQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetEventsByCurrentUserQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnEvents_WhenUserHasEvents()
    {
        var cancellationToken = CancellationToken.None;

        var events = new List<Event>
        {
            new Event("Event 1", null, DateTime.Now, 10, null, new Place("Place 1", "PLACE 1"), null) { Id = 1 },
            new Event("Event 2", null, DateTime.Now, 10, null, new Place("Place 2", "PLACE 2"), null) { Id = 2 }
        };

        var email = "test@gmail.com";
        var person = new Person("Name", "Surname", new DateTime(2000, 1, 1));

        var participations = new List<Participant>
        {
            new Participant(email, person, events[0]),
            new Participant(email, person, events[1]),
        };

        var query = new GetEventsByCurrentUserQuery(email, 1, 10);

        _unitOfWorkMock.Setup(u => 
            u.ParticipantsRepository.ListAsync(It.IsAny<Expression<Func<Participant, bool>>>(), cancellationToken))
            .ReturnsAsync(participations);

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.PaginatedListAsync(It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken))
            .ReturnsAsync(events);

        var result = await _handler.Handle(query, cancellationToken);

        result.Should().BeEquivalentTo(events);
        _unitOfWorkMock.Verify(u => 
            u.ParticipantsRepository.ListAsync(It.IsAny<Expression<Func<Participant, bool>>>(), cancellationToken), 
            Times.Once);

        _unitOfWorkMock.Verify(u => 
            u.EventsRepository.PaginatedListAsync(It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenUserHasNoEvents()
    {
        var email = "test@gmail.com";
        var cancellationToken = CancellationToken.None;
        var query = new GetEventsByCurrentUserQuery(email, 1, 10);

        _unitOfWorkMock.Setup(u => 
            u.ParticipantsRepository.ListAsync(It.IsAny<Expression<Func<Participant, bool>>>(), cancellationToken))
            .ReturnsAsync(new List<Participant>());

        _unitOfWorkMock.Setup(u => 
            u.EventsRepository.PaginatedListAsync(It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken))
            .ReturnsAsync(new List<Event>());

        var result = await _handler.Handle(query, cancellationToken);

        result.Should().BeEmpty();
        
        _unitOfWorkMock.Verify(u => 
            u.ParticipantsRepository.ListAsync(It.IsAny<Expression<Func<Participant, bool>>>(), cancellationToken), 
            Times.Once);
        
        _unitOfWorkMock.Verify(u =>
            u.EventsRepository.PaginatedListAsync(It.IsAny<Expression<Func<Event, bool>>>(), 0, 10, cancellationToken),
            Times.Once);
    }

}
