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
    }
}
