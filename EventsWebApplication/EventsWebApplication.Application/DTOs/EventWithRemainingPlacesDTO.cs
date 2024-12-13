using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Application.DTOs;

public record EventWithRemainingPlacesDTO(
    string Title,
    string? Description,
    DateTime EventDateTime,
    int ParticipantsMaxCount,
    int PlacesRemain,
    string PlaceName,
    string? CategoryName);
