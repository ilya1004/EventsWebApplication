using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;

namespace EventsWebApplication.API.Mapping.EventsMappingProfiles;

public class CreateEventMappingProfile : Profile
{
    public CreateEventMappingProfile()
    {
        CreateMap<CreateEventRequest, CreateEventCommand>();
    }
}
