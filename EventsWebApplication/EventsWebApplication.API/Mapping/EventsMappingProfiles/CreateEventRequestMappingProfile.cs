using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

namespace EventsWebApplication.API.Mapping.EventsMappingProfiles;

public class CreateEventRequestMappingProfile : Profile
{
    public CreateEventRequestMappingProfile()
    {
        CreateMap<CreateEventRequest, CreateEventCommand>()
            .ConstructUsing(e => new CreateEventCommand(
                e.EventDTO, e.ImageFile == null ? null : e.ImageFile.OpenReadStream(), e.ImageFile == null ? null : e.ImageFile.ContentType));
            //.ForMember(dest => dest.EventDTO, opt =>
            //    opt.MapFrom(src => src.EventDTO))
            //.ForMember(dest => dest.FileStream, opt =>
            //    opt.MapFrom(src => src.ImageFile == null ? null : src.ImageFile.OpenReadStream()))
            //.ForMember(dest => dest.ContentType, opt =>
            //    opt.MapFrom(src => src.ImageFile == null ? null : src.ImageFile.ContentType));
    }
}
