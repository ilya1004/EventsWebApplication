using EventsWebApplication.API.Utils;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.AddParticipantToEvent;
using EventsWebApplication.Application.UseCases.UserUseCases.Queries.GetCurrentUserInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading;

namespace EventsWebApplication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = AuthPolicies.UserPolicy)]
    public async Task<IActionResult> GetCurrentUserInfo(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCurrentUserInfoQuery(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
            Request.Headers.Authorization.FirstOrDefault()!.Split(' ')[1]),
            cancellationToken);

        return Ok(result);
    }
}
