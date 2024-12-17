using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Abstractions.Data;
using System.Runtime.CompilerServices;

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

        

        //var result = events.Zip(values)
        //        .Select(item => new EventWithRemainingPlacesDTO(
        //            item.First.Id,
        //            item.First.Title, 
        //            item.First.Description, 
        //            item.First.EventDateTime,
        //            item.First.ParticipantsMaxCount,
        //            item.First.ParticipantsMaxCount - values.FirstOrDefault(val => item.First.Id == val.EventId, defaultValue).Count,
        //            item.First.Place.Name,
        //            item.First.Category?.Name));

        return result;
    }
}
