using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.Mapping.EventsMappingProfiles;

public class EventWithRemainingPlacesMappingProfile : Profile
{
    public EventWithRemainingPlacesMappingProfile()
    {
        CreateMap<Event, EventWithRemainingPlacesDTO>()
            .ConstructUsing(src => new EventWithRemainingPlacesDTO(
                src.Id,
                src.Title,
                src.Description,
                src.EventDateTime,
                src.ParticipantsMaxCount,
                src.ParticipantsMaxCount - src.Participants.Count,
                src.Place.Name,
                src.Category == null ? null : src.Category.Name));
    }
}
