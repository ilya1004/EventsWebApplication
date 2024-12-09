namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.AddParticipantToEvent;

public sealed record AddParticipantToEventCommand(string UserId, int EventId, string Token) : IRequest;
