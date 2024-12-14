namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Queries.CheckParticipationInEvent;

public sealed record CheckParticipationInEventQuery(string Email, int EventId) : IRequest<bool>;