using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsListAll;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsWithRemainingPlaces;

namespace EventsWebApplication.API.Mapping.EventsMappingProfiles;

public class GetEventsListAllRequestMappingProfile : Profile
{
    public GetEventsListAllRequestMappingProfile()
    {
        CreateMap<GetEventsListAllRequest, GetEventsListAllQuery>();
        CreateMap<GetEventsListAllRequest, GetEventsWithRemainingPlacesQuery>();
    }
}
