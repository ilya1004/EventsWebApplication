using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByFilter;

namespace EventsWebApplication.API.Mapping.EventsMappingProfiles;

public class GetEventsByFilterMappingProfile : Profile
{
    public GetEventsByFilterMappingProfile()
    {
        CreateMap<GetEventsByFilterRequest, GetEventsByFilterQuery>();
    }
}
