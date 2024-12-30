using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;

namespace EventsWebApplication.API.Mapping.EventsMappingProfiles;

public class UpdateEventRequestMappingProfile : Profile
{
    public UpdateEventRequestMappingProfile()
    {
        CreateMap<UpdateEventRequest, UpdateEventCommand>()
            .ConstructUsing(e => new UpdateEventCommand(
                e.Id, e.EventDTO, e.ImageFile == null ? null : e.ImageFile.OpenReadStream(), e.ImageFile == null ? null : e.ImageFile.ContentType));
    }
}
