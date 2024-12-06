using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Application.DTOs;

public record EventDTO(
    string Title,
    string? Description,
    DateTime EventDateTime,
    int ParticipantsMaxCount,
    IFormFile? ImageFile,
    string PlaceName,
    string? CategoryName);
