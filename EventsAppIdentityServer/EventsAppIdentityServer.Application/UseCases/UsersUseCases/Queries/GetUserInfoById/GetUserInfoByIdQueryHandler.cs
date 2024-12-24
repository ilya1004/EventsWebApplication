using EventsAppIdentityServer.Application.DTOs;
using EventsAppIdentityServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace EventsAppIdentityServer.Application.UseCases.UsersUseCases.Queries.GetUserInfoById;

public class GetUserInfoByIdQueryHandler : IRequestHandler<GetUserInfoByIdQuery, UserInfoDTO>
{
    private readonly UserManager<AppUser> _userManager;

    public GetUserInfoByIdQueryHandler(UserManager<AppUser> userManager,)
    {
        _userManager = userManager;
    }

    public async Task<UserInfoDTO> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.AuthHeader) || !request.AuthHeader.StartsWith("Bearer "))
        {
            throw new UnauthorizedAccessException("Access token not found");
        }

        var token = request.AuthHeader["Bearer ".Length..].Trim();

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken;
        try
        {
            jwtToken = handler.ReadJwtToken(token);
        }
        catch (Exception)
        {
            throw new UnauthorizedAccessException("Invalid token");
        }

        var userIdFromToken = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var userRoleFromToken = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        if (string.IsNullOrEmpty(userIdFromToken))
        {
            throw new UnauthorizedAccessException("Token does not contain user ID");
        }

        if (string.IsNullOrEmpty(userRoleFromToken))
        {
            throw new UnauthorizedAccessException("Token does not contain user role");
        }

        if (userIdFromToken != request.Id && userRoleFromToken != "Admin")
        {
            //throw new forbidden
        }

        var user = await _userManager.FindByIdAsync(request.Id);

        if (user == null)
        {
            //throw new not found
        }

        var userData = new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.Name,
            user.Surname,
            user.Birthday
        };

        var userInfoDTO = 

           

        return
    }
}
