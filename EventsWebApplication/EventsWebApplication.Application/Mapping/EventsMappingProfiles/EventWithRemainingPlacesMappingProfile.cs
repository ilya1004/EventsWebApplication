using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.Mapping.EventsMappingProfiles;

public class EventWithRemainingPlacesMappingProfile : Profile
{
    public EventWithRemainingPlacesMappingProfile()
    {
        CreateMap<Event, EventWithRemainingPlacesDTO>()
            .ForMember(e => e.PlacesRemain, opt =>
                opt.MapFrom(e => e.ParticipantsMaxCount - e.Participants.Count))
            .ForMember(e => e.PlaceName, opt => 
                opt.MapFrom(e => e.Place.Name))
            .ForMember(e => e.CategoryName, opt =>
                opt.MapFrom(e => e.Category == null ? null : e.Category.Name));
    }
}
