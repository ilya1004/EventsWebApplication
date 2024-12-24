using AutoMapper;
using EventsAppIdentityServer.Application.DTOs;
using EventsAppIdentityServer.Application.Exceptions;
using EventsAppIdentityServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace EventsAppIdentityServer.Application.UseCases.UsersUseCases.Queries.GetUserInfoById;

public class GetUserInfoByIdQueryHandler : IRequestHandler<GetUserInfoByIdQuery, UserInfoDTO>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public GetUserInfoByIdQueryHandler(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserInfoDTO> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.AuthHeader) || !request.AuthHeader.StartsWith("Bearer "))
        {
            throw new UnauthorizedException("Access token not found");
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
            throw new UnauthorizedException("Invalid token");
        }

        var userIdFromToken = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var userRoleFromToken = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        if (string.IsNullOrEmpty(userIdFromToken))
        {
            throw new UnauthorizedException("Token does not contain user ID");
        }

        if (string.IsNullOrEmpty(userRoleFromToken))
        {
            throw new UnauthorizedException("Token does not contain user role");
        }

        if (userIdFromToken != request.Id && userRoleFromToken != "Admin")
        {
            throw new ForbiddenException($"Access to user information by User ID {userIdFromToken} is not allowed");
        }

        var user = await _userManager.FindByIdAsync(request.Id);

        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.Id} not found");
        }

        var userInfoDTO = _mapper.Map<UserInfoDTO>(user);

        return userInfoDTO;
    }
}
