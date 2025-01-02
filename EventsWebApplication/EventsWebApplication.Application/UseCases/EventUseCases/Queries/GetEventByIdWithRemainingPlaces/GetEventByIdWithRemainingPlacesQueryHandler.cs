using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Exceptions;
using EventsWebApplication.Domain.Abstractions.Data;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventByIdWithRemainingPlaces;

public class GetEventByIdWithRemainingPlacesQueryHandler : IRequestHandler<GetEventByIdWithRemainingPlacesQuery, EventWithRemainingPlacesDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEventByIdWithRemainingPlacesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<EventWithRemainingPlacesDTO> Handle(GetEventByIdWithRemainingPlacesQuery query, CancellationToken cancellationToken)
    {
        var eventObj = await _unitOfWork.EventsRepository.GetByIdAsync(query.EventId, cancellationToken, e => e.Participants);

        if (eventObj is null)
        {
            throw new NotFoundException($"Event with ID {query.EventId} not found.");
        }

        var eventWithRemainingPlaces = _mapper.Map<EventWithRemainingPlacesDTO>(eventObj);

        return eventWithRemainingPlaces;
    }
}
