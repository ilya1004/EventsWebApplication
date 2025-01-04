using EventsAppIdentityServer.Domain.Abstractions;
using EventsAppIdentityServer.Domain.Entities;
using EventsAppIdentityServer.Domain.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EventsAppIdentityServer.Infrastructure.Data;

public class DbInitializer : IDbInitializer
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public DbInitializer(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeDb()
    {
        if (await _roleManager.FindByNameAsync(AppRoles.AdminRole) == null)
        {
            await _roleManager.CreateAsync(new IdentityRole(AppRoles.AdminRole));
            await _roleManager.CreateAsync(new IdentityRole(AppRoles.UserRole));
        }
        else
        {
            return;
        }

        AppUser admin = new()
        {
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            Name = "Admin",
            Surname = "Admin",
        };

        await _userManager.CreateAsync(admin, "Admin_123");
        await _userManager.AddToRoleAsync(admin, AppRoles.AdminRole);
        await _userManager.AddClaimsAsync(admin,
            [
                new Claim(JwtClaimTypes.Id, admin.Id),
                new Claim(JwtClaimTypes.Email, admin.Email),
                new Claim(JwtClaimTypes.Role, AppRoles.AdminRole)
            ]);


        AppUser user1 = new()
        {
            UserName = "ilya@gmail.com",
            Email = "ilya@gmail.com",
            EmailConfirmed = true,
            Name = "Ilya",
            Surname = "Rabets",
            Birthday = new DateTime(2004, 9, 16, 0, 0, 0, DateTimeKind.Utc),
        };

        await _userManager.CreateAsync(user1, "Ilya_123");
        await _userManager.AddToRoleAsync(user1, AppRoles.UserRole);
        await _userManager.AddClaimsAsync(user1,
            [
                new Claim(JwtClaimTypes.Id, user1.Id),
                new Claim(JwtClaimTypes.Email, user1.Email),
                new Claim(JwtClaimTypes.Role, AppRoles.UserRole)
            ]);

        
        AppUser user2 = new()
        {
            UserName = "anna@gmail.com",
            Email = "anna@gmail.com",
            EmailConfirmed = true,
            Name = "Anna",
            Surname = "Petrova",
            Birthday = new DateTime(1995, 4, 23, 0, 0, 0, DateTimeKind.Utc),
        };

        await _userManager.CreateAsync(user2, " ");
        await _userManager.AddToRoleAsync(user2, AppRoles.UserRole);
        await _userManager.AddClaimsAsync(user2,
            [
                new Claim(JwtClaimTypes.Id, user2.Id),
                new Claim(JwtClaimTypes.Email, user2.Email),
                new Claim(JwtClaimTypes.Role, AppRoles.UserRole)
            ]);


        AppUser user3 = new()
        {
            UserName = "dmitry@gmail.com",
            Email = "dmitry@gmail.com",
            EmailConfirmed = true,
            Name = "Dmitry",
            Surname = "Ivanov",
            Birthday = new DateTime(1988, 12, 5, 0, 0, 0, DateTimeKind.Utc),
        };

        await _userManager.CreateAsync(user3, "Dmitry_123");
        await _userManager.AddToRoleAsync(user3, AppRoles.UserRole);
        await _userManager.AddClaimsAsync(user3,
            [
                new Claim(JwtClaimTypes.Id, user3.Id),
                new Claim(JwtClaimTypes.Email, user3.Email),
                new Claim(JwtClaimTypes.Role, AppRoles.UserRole)
            ]);
    }
}
