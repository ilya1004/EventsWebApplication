﻿using EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.AddParticipantToEvent;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.RemoveParticipantFromEvent;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Queries.GetParticipantById;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Queries.GetParticipantsByEventId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventsWebApplication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParticipantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetParticipantById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetParticipantByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("by-event/{id}")]
    public async Task<IActionResult> GetParticipantByEventId(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetParticipantsByEventIdQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Route("{eventId}")]
    [Authorize(Policy = "User")]
    public async Task<IActionResult> AddMyParticipationInEvent(int eventId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddParticipantToEventCommand(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
            eventId,
            Request.Headers.Authorization.FirstOrDefault()!.Split(' ')[1]), 
            cancellationToken);

        return Ok();
    }

    [HttpDelete]
    [Route("{eventId}")]
    [Authorize(Policy = "User")]
    public async Task<IActionResult> CancelParticipationInEvent(int eventId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveParticipantFromEventCommand(
            User.FindFirst(ClaimTypes.Email)!.Value,
            eventId,
            Request.Headers.Authorization.FirstOrDefault()!.Split(' ')[1]),
            cancellationToken);

        return Ok();
    }
}
