using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;

namespace EventsWebApplication.API.Mapping.EventsMappingProfiles;

public class UpdateEventRequestMappingProfile : Profile
{
    public UpdateEventRequestMappingProfile()
    {
        CreateMap<UpdateEventRequest, UpdateEventCommand>();
    }
}
