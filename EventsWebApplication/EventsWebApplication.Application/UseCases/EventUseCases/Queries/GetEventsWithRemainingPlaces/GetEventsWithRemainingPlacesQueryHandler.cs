using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsWithRemainingPlaces;

public class GetEventsWithRemainingPlacesQueryHandler : IRequestHandler<GetEventsWithRemainingPlacesQuery, IEnumerable<EventWithRemainingPlacesDTO>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEventsWithRemainingPlacesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EventWithRemainingPlacesDTO>> Handle(GetEventsWithRemainingPlacesQuery request, CancellationToken cancellationToken)
    {
        int offset = (request.PageNo - 1) * request.PageSize;

        var events = await _unitOfWork.EventsRepository.PaginatedListAllAsync(offset, request.PageSize, cancellationToken);

        var values = await _unitOfWork.ParticipantsRepository.CountParticipantsByEvents(cancellationToken);

        (int EventId, int Count) defaultValue = (0, 0);
        IEnumerable<EventWithRemainingPlacesDTO> result = [];

        foreach (var item in events)
        {
            var placesRemain = values.FirstOrDefault(val => item.Id == val.EventId, defaultValue).Count;
            
            result = result.Append(new EventWithRemainingPlacesDTO(
                item.Id,
                item.Title,
                item.Description,
                item.EventDateTime,
                item.ParticipantsMaxCount,
                placesRemain,
                item.Place.Name,
                item.Category?.Name));
        }

        return result;
    }
}
