using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Application.DTOs;

public record EventDTO(
    string Title,
    string? Description,
    DateTime EventDateTime,
    int ParticipantsMaxCount,
    string PlaceName,
    string? CategoryName);
