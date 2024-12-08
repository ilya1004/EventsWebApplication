using EventsWebApplication.Domain.Entities.Participants;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Queries.GetParticipantsByEventId;

public sealed record GetParticipantsByEventIdQuery(int EventId, int PageNo = 1, int PageSize = 20) : IRequest<IEnumerable<Participant>>;
