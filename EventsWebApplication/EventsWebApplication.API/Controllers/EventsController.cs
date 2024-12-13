using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.API.Utils;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.DeleteEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByCurrentUser;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDate;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDateRange;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsListAll;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsWithRemainingPlaces;
using EventsWebApplication.Application.UseCases.UserUseCases.Queries.GetCurrentUserInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;

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
    // disable antiforgery
    public async Task<IActionResult> CreateEvent([FromForm] CreateEventRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(_mapper.Map<CreateEventCommand>(request), cancellationToken);

        return Ok();
    }

    [HttpGet("admin")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> GetAllEvents1([FromQuery] GetEventsListAllRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsListAllQuery>(request), cancellationToken);

        return Ok(result);
    }

    [HttpGet("user")]
    [Authorize(Policy = "User")]
    public async Task<IActionResult> GetAllEvents2([FromQuery] GetEventsListAllRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsListAllQuery>(request), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEvents([FromQuery] GetEventsListAllRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsListAllQuery>(request), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("with-remaining-places")]
    public async Task<IActionResult> GetAllEventsWithRemainingPlaces([FromQuery] GetEventsListAllRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsWithRemainingPlacesQuery>(request), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("by-date")]
    public async Task<IActionResult> GetEventsByDate([FromQuery] GetEventsByDateRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsByDateQuery>(request), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("by-date-range")]
    public async Task<IActionResult> GetEventsByDateRage([FromQuery] GetEventsByDateRangeRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsByDateRangeQuery>(request), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetEventById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEventByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}/with-participants")]
    public async Task<IActionResult> GetEventByIdWithParticipants(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEventByIdQuery(id, e => e.Participants), cancellationToken);

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
    public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(_mapper.Map<UpdateEventCommand>(request), cancellationToken);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteEvent([FromQuery] int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteEventCommand(id), cancellationToken);

        return Ok();
    }
}
