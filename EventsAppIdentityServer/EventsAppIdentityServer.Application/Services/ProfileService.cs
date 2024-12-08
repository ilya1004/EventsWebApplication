using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using EventsAppIdentityServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventsAppIdentityServer.Application.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ProfileService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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
        context.IsActive = true; // Убедитесь, что пользователь активен
        return Task.CompletedTask;
    }
}
