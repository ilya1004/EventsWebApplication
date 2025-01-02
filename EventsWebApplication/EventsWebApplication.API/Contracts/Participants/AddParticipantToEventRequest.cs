using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.API.Contracts.Participants;

public sealed record AddParticipantToEventRequest(
    int EventId,
    UserInfoDTO UserInfoDTO);
