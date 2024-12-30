namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.RemoveParticipantFromEvent;

public sealed record RemoveParticipantFromEventCommand(string Email, int EventId): IRequest;