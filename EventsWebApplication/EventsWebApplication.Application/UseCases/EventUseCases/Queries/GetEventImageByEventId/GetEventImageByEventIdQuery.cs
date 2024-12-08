using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventImageByEventId;

public sealed record GetEventImageByEventIdQuery(int EventId) : IRequest<FileResponseDTO>;
