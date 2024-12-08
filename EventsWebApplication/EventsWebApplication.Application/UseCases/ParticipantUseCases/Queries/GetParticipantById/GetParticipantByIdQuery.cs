using EventsWebApplication.Domain.Entities.Participants;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Queries.GetParticipantById;

public sealed record GetParticipantByIdQuery(int Id) : IRequest<Participant>;