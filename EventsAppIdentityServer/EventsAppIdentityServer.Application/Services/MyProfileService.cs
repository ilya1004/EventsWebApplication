using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using EventsAppIdentityServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EventsAppIdentityServer.Application.Services;

public class MyProfileService : IProfileService
{
    private readonly UserManager<AppUser> _userManager;

    public MyProfileService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        
        if (user != null)
        {
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            List<Claim> claims = context.Caller switch
            {
                IdentityServerConstants.ProfileDataCallers.ClaimsProviderAccessToken => [
                    new Claim("email", user.Email!),
                    new Claim("role", role!)
                    ],
                IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint => [
                    new Claim("email", user.Email!),
                    new Claim("role", role!),
                    new Claim("name", user.Name),
                    new Claim("surname", user.Surname),
                    new Claim("birthday", user.Birthday.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")),
                    ],
                _ => []
                
            };

            context.IssuedClaims.AddRange(claims);
        }
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.FromResult(context.IsActive);
    }
}
