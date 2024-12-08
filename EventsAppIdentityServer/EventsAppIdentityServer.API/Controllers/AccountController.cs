using Microsoft.AspNetCore.Mvc;
using MediatR;
using EventsAppIdentityServer.Application.UseCases.UsersUseCases;
using EventsAppIdentityServer.Application.DTOs;

namespace EventsAppIdentityServer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO userDTO)
    {
        await _mediator.Send(new RegisterUserCommand(userDTO));

        return Ok();
    }
}
