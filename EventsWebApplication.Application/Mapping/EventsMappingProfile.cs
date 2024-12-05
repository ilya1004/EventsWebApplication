using AutoMapper;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

namespace EventsWebApplication.Application.Mapping;

public class EventsMappingProfile : Profile
{
    public EventsMappingProfile()
    {
        CreateMap<CreateEventCommand, Event>()
            .ForMember(e => e.Place, opt =>
                opt.MapFrom(c => new Place(c.PlaceName, c.PlaceName.ToUpper())))
            .ForMember(e => e.Category, opt =>
                opt.MapFrom(c => c.CategoryName != null ? new Category(c.CategoryName, c.CategoryName.ToUpper()) : null));
    }
}
