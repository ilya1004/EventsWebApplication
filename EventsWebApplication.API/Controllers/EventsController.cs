using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventsByDate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;
    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("by-date")]
    public async Task<IActionResult> GetEventListsByDate([FromQuery] GetEventsByDateQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }
}
