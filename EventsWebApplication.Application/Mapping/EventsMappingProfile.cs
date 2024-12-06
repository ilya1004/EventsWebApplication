using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

namespace EventsWebApplication.Application.Mapping;

public class EventsMappingProfile : Profile
{
    public EventsMappingProfile()
    {
        //CreateMap<CreateEventCommand, Event>()
        //    .ForMember(e => e.Place, opt =>
        //        opt.MapFrom(c => new Place(c.EventDTO.PlaceName, c.EventDTO.PlaceName.ToUpper())))
        //    .ForMember(e => e.Category, opt =>
        //        opt.MapFrom(c => c.EventDTO.CategoryName != null ? 
        //                         new Category(c.EventDTO.CategoryName, c.EventDTO.CategoryName.ToUpper()) : 
        //                         null));

        CreateMap<EventDTO, Event>()
            .ForMember(e => e.Place, opt =>
                opt.MapFrom(c => new Place(c.PlaceName, c.PlaceName.ToUpper())))
            .ForMember(e => e.Category, opt =>
                opt.MapFrom(c => c.CategoryName != null ?
                                 new Category(c.CategoryName, c.CategoryName.ToUpper()) :
                                 null))
            .ForMember(e => e.Image, opt =>
                opt.MapFrom(dto => dto.ImageFile!.FileName));

    }
}
