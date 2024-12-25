using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.API.Utils;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeleteEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventByIdWithRemainingPlaces;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByCurrentUser;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDateRange;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByFilter;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByTitle;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsListAll;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsWithRemainingPlaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EventsWebApplication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("ReactClientCors")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public EventsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
        
    }


    [HttpPost]
    [Authorize(Policy = AuthPolicies.AdminPolicy)]
    public async Task<IActionResult> CreateEvent([FromForm] CreateEventRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(_mapper.Map<CreateEventCommand>(request), cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Authorize(Policy = AuthPolicies.AdminOrUserPolicy)]
    public async Task<IActionResult> GetAllEvents([FromQuery] GetEventsListAllRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsListAllQuery>(request), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("with-remaining-places")]
    [Authorize(Policy = AuthPolicies.AdminOrUserPolicy)]
    public async Task<IActionResult> GetAllEventsWithRemainingPlaces([FromQuery] GetEventsListAllRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsWithRemainingPlacesQuery>(request), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("by-date-range")]
    [Authorize(Policy = AuthPolicies.AdminOrUserPolicy)]
    public async Task<IActionResult> GetEventsByDateRange([FromQuery] GetEventsByDateRangeRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsByDateRangeQuery>(request), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("by-title")]
    [Authorize(Policy = AuthPolicies.AdminOrUserPolicy)]
    public async Task<IActionResult> GetEventsByTitle([FromQuery] GetEventsByTitleRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsByTitleQuery>(request), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("by-filter")]
    [Authorize(Policy = AuthPolicies.AdminOrUserPolicy)]
    public async Task<IActionResult> GetEventsByFilter([FromQuery] GetEventsByFilterRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsByFilterQuery>(request), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Policy = AuthPolicies.AdminOrUserPolicy)]
    public async Task<IActionResult> GetEventById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEventByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}/with-participants")]
    [Authorize(Policy = AuthPolicies.AdminPolicy)]
    public async Task<IActionResult> GetEventByIdWithParticipants(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEventByIdQuery(id, e => e.Participants), cancellationToken);

        return Ok(
            JsonConvert.SerializeObject(result,
            settings: new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
    }

    [HttpGet]
    [Route("{id:int}/with-remaining-places")]
    [Authorize(Policy = AuthPolicies.AdminOrUserPolicy)]
    public async Task<IActionResult> GetEventByIdWithRemainingPlaces(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEventByIdWithRemainingPlacesQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("by-current-user")]
    [Authorize(Policy = AuthPolicies.UserPolicy)]
    public async Task<IActionResult> GetEventByCurrentUser(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEventsByCurrentUserQuery(
            User.FindFirst(ClaimTypes.Email)!.Value), cancellationToken);

        return Ok(result);
    }

    [HttpPut]
    [Authorize(Policy = AuthPolicies.AdminPolicy)]
    public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(_mapper.Map<UpdateEventCommand>(request), cancellationToken);

        return Ok();
    }

    [HttpDelete]
    [Authorize(Policy = AuthPolicies.AdminPolicy)]
    public async Task<IActionResult> DeleteEvent([FromQuery] int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteEventCommand(id), cancellationToken);

        return Ok();
    }
}
