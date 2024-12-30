using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.API.Contracts.Events;

public sealed record AddParticipantToEventRequest(
    int EventId,
    UserInfoDTO UserInfoDTO);
