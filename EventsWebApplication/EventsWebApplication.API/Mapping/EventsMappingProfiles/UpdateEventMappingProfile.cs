using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;

namespace EventsWebApplication.API.Mapping.EventsMappingProfiles;

public class UpdateEventMappingProfile : Profile
{
    public UpdateEventMappingProfile()
    {
        CreateMap<UpdateEventRequest, UpdateEventCommand>();
    }
}
