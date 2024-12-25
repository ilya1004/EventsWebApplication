using Microsoft.AspNetCore.Mvc;
using MediatR;
using EventsAppIdentityServer.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using EventsAppIdentityServer.Domain.Entities;
using EventsAppIdentityServer.Application.UseCases.UsersUseCases.Commands.RegisterUser;
using EventsAppIdentityServer.Application.UseCases.UsersUseCases.Queries.GetUserInfoById;

namespace EventsAppIdentityServer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO userDTO)
    {
        await _mediator.Send(new RegisterUserCommand(userDTO));

        return Ok();
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {

        var result = await _mediator.Send(new GetUserInfoByIdQuery(
            id, 
            Request.Headers.Authorization.FirstOrDefault()));

        return Ok(result);
    }

}
