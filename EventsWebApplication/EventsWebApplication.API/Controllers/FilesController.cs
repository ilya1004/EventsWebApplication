using EventsWebApplication.API.Utils;
using EventsWebApplication.Application.UseCases.EventUseCases.Queries.GetEventImageByEventId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("by-event-id/{eventId}")]
    [Authorize(Policy = AuthPolicies.AdminOrUserPolicy)]
    public async Task<IActionResult> GetFileByEventId(int eventId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEventImageByEventIdQuery(eventId), cancellationToken);

        return File(result.Stream, result.ContentType);
    }
}
