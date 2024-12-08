using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using EventsAppIdentityServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
            var claims = new List<Claim>
            {
                new Claim("name", user.Name!),
                new Claim("surname", user.Surname!),
                new Claim("birthday", user.Birthday.ToString()),
                new Claim("email", user.Email!),
                new Claim("role", role!)
            };

            context.IssuedClaims.AddRange(claims);
        }
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        return Task.CompletedTask;
    }
}
