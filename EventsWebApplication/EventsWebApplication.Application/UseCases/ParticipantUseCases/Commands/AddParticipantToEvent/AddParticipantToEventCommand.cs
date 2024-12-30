using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.AddParticipantToEvent;

public sealed record AddParticipantToEventCommand(int EventId, string Email, UserInfoDTO UserInfoDTO) : IRequest;
