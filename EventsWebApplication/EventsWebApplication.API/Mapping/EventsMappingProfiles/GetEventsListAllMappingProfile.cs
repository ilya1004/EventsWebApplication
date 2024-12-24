using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsListAll;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsWithRemainingPlaces;

namespace EventsWebApplication.API.Mapping.EventsMappingProfiles;

public class GetEventsListAllMappingProfile : Profile
{
    public GetEventsListAllMappingProfile()
    {
        CreateMap<GetEventsListAllRequest, GetEventsListAllQuery>();
        CreateMap<GetEventsListAllRequest, GetEventsWithRemainingPlacesQuery>();
    }
}
