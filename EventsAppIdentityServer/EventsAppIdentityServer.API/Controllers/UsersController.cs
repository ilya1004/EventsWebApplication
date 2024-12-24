using Microsoft.AspNetCore.Mvc;
using MediatR;
using EventsAppIdentityServer.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using EventsAppIdentityServer.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using EventsAppIdentityServer.Application.UseCases.UsersUseCases.Commands.RegisterUser;
using EventsAppIdentityServer.Application.UseCases.UsersUseCases.Queries.GetUserInfoById;

namespace EventsAppIdentityServer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly UserManager<AppUser> _userManager;

    public UsersController(IMediator mediator, UserManager<AppUser> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
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

        //if (!Request.Headers.ContainsKey("Authorization"))
        //{
        //    return Unauthorized();
        //}

        //var authHeader = Request.Headers.Authorization.FirstOrDefault();
        //if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        //{
        //    return Unauthorized();
        //}

        //var token = authHeader["Bearer ".Length..].Trim();

        //var handler = new JwtSecurityTokenHandler();
        //JwtSecurityToken jwtToken;
        //try
        //{
        //    jwtToken = handler.ReadJwtToken(token);
        //}
        //catch (Exception)
        //{
        //    return Unauthorized("Invalid token");
        //}

        //var userIdFromToken = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        //var userRoleFromToken = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        //if (string.IsNullOrEmpty(userIdFromToken))
        //{
        //    return Unauthorized("Token does not contain user ID");
        //}

        //if (userIdFromToken != id && userRoleFromToken != "Admin")
        //{
        //    return Forbid();
        //}

        //var user = await _userManager.FindByIdAsync(id);

        //if (user == null)
        //{
        //    return NoContent();
        //}

        //var userData = new
        //{
        //    user.Id,
        //    user.UserName,
        //    user.Email,
        //    user.Name,
        //    user.Surname,
        //    user.Birthday
        //};

        //return Ok(userData);
    }

}
