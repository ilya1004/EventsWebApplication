﻿using AutoMapper;
using EventsWebApplication.API.Contracts.Events;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.CreateEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Commands.UpdateEvent;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventById;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDate;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDateRange;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsListAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace EventsWebApplication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> CreateEvent([FromQuery] CreateEventRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(_mapper.Map<CreateEventCommand>(request), cancellationToken);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEvents([FromQuery] GetEventsListAllRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<GetEventsListAllQuery>(request), cancellationToken);

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

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateEvent(int id, EventDTO eventDTO, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateEventCommand(id, eventDTO), cancellationToken);

        return Ok();
    }
}