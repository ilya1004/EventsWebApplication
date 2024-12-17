using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventByIdWithRemainingPlaces;

public class GetEventByIdWithRemainingPlacesQueryHandler : IRequestHandler<GetEventByIdWithRemainingPlacesQuery, EventWithRemainingPlacesDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEventByIdWithRemainingPlacesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<EventWithRemainingPlacesDTO> Handle(GetEventByIdWithRemainingPlacesQuery query, CancellationToken cancellationToken)
    {
        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(query.EventId, cancellationToken, e => e.Participants);

        if (eventObj == null)
        {
            throw new Exception($"Event with ID {query.EventId} not found.");
        }

        var eventWithRemainingPlaces = new EventWithRemainingPlacesDTO(
            eventObj.Id,
            eventObj.Title,
            eventObj.Description,
            eventObj.EventDateTime,
            eventObj.ParticipantsMaxCount,
            eventObj.ParticipantsMaxCount - eventObj.Participants.Count,
            eventObj.Place.Name,
            eventObj.Category?.Name);

        return eventWithRemainingPlaces;
    }
}
