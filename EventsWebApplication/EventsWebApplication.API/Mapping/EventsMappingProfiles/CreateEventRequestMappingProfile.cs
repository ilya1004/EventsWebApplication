using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

namespace EventsWebApplication.API.Mapping.EventsMappingProfiles;

public class CreateEventRequestMappingProfile : Profile
{
    public CreateEventRequestMappingProfile()
    {
        CreateMap<CreateEventRequest, CreateEventCommand>()
            .ForMember(dest => dest.FileStream, opt => 
                opt.MapFrom(src => src.ImageFile.OpenReadStream()))
            .ForMember(dest => dest.ContentType, opt =>
                opt.MapFrom(src => src.ImageFile.ContentType));
    }
}
