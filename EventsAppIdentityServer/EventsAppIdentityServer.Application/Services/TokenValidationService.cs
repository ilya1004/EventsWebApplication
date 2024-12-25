using EventsAppIdentityServer.Application.Exceptions;
using EventsAppIdentityServer.Domain.Abstractions;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace EventsAppIdentityServer.Application.Services;

public class TokenValidationService : ITokenValidationService
{
    public void ValidateAuthHeader(string? authHeader, string userId)
    {
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            throw new UnauthorizedException("Access token not found");
        }

        var token = authHeader["Bearer ".Length..].Trim();

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

        if (userIdFromToken != userId && userRoleFromToken != "Admin")
        {
            throw new ForbiddenException($"Access to user information by User ID {userIdFromToken} is not allowed");
        }
    }
}
