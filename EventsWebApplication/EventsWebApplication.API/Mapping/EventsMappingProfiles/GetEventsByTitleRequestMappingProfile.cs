using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByTitle;

namespace EventsWebApplication.API.Mapping.EventsMappingProfiles;

public class GetEventsByTitleRequestMappingProfile : Profile
{
    public GetEventsByTitleRequestMappingProfile()
    {
        CreateMap<GetEventsByTitleRequest, GetEventsByTitleQuery>();
    }
}
