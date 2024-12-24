using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDateRange;

namespace EventsWebApplication.API.Mapping.EventsMappingProfiles;

public class GetEventsByDateRangeMappingProfile : Profile
{
    public GetEventsByDateRangeMappingProfile()
    {
        CreateMap<GetEventsByDateRangeRequest, GetEventsByDateRangeQuery>();
    }
}
