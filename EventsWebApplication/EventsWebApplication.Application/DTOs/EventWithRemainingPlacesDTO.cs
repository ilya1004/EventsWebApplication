using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Application.DTOs;

public record EventWithRemainingPlacesDTO(
    int Id,
    string Title,
    string? Description,
    DateTime EventDateTime,
    int ParticipantsMaxCount,
    int PlacesRemain,
    string PlaceName,
    string? CategoryName);
