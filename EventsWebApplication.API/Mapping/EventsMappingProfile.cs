using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDate;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDateRange;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsListAll;

namespace EventsWebApplication.API.Mapping;

public class EventsMappingProfile : Profile
{
    public EventsMappingProfile()
    {
        CreateMap<CreateEventRequest, CreateEventCommand>();
        CreateMap<GetEventsListAllRequest, GetEventsListAllQuery>();
        CreateMap<GetEventsByDateRequest, GetEventsByDateQuery>();
        CreateMap<GetEventsByDateRangeRequest, GetEventsByDateRangeQuery>();
    }
}
