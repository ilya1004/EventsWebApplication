using EventsWebApplication.Application.UseCases.ParticipantUseCases.Queries.GetParticipantById;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Queries.GetParticipantsByEventId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    [HttpDelete]
    [Route("{eventId}")]
    public async Task<IActionResult> CancelParticipationInEvent(int eventId, CancellationToken cancellationToken)
    {
        return Ok();
    }
}
