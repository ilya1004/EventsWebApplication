using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

namespace EventsWebApplication.Application.Mapping.EventsMappingProfiles;

public class CreateEventCommandMappingProfile : Profile
{
    public CreateEventCommandMappingProfile()
    {
        CreateMap<CreateEventCommand, Event>()
            .ForMember(e => e.Place, opt =>
                opt.MapFrom(c => new Place(c.EventDTO.PlaceName, c.EventDTO.PlaceName.ToUpper())))
            .ForMember(e => e.Category, opt =>
                opt.MapFrom(c => c.EventDTO.CategoryName != null ?
                                 new Category(c.EventDTO.CategoryName, c.EventDTO.CategoryName.ToUpper()) :
                                 null));
    }
}
