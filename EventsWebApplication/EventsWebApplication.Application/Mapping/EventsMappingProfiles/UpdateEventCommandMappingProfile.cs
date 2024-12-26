using EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;

namespace EventsWebApplication.Application.Mapping.EventsMappingProfiles;

public class UpdateEventCommandMappingProfile : Profile
{
    public UpdateEventCommandMappingProfile()
    {
        CreateMap<UpdateEventCommand, Event>()
            .ForMember(dest => dest.Title, opt =>
                opt.MapFrom(src => src.EventDTO.Title))
            .ForMember(dest => dest.Description, opt =>
                opt.MapFrom(src => src.EventDTO.Description))
            .ForMember(dest => dest.EventDateTime, opt =>
                opt.MapFrom(src => src.EventDTO.EventDateTime))
            .ForMember(dest => dest.ParticipantsMaxCount, opt =>
                opt.MapFrom(src => src.EventDTO.ParticipantsMaxCount))
            .ForMember(e => e.Place, opt =>
                opt.MapFrom(c => new Place(c.EventDTO.PlaceName, c.EventDTO.PlaceName.ToUpper())))
            .ForMember(e => e.Category, opt =>
                opt.MapFrom(c => c.EventDTO.CategoryName != null ?
                                 new Category(c.EventDTO.CategoryName, c.EventDTO.CategoryName.ToUpper()) :
                                 null));
    }
}
