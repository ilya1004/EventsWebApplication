using EventsAppIdentityServer.Domain.Abstractions;
using EventsAppIdentityServer.Domain.Entities;
using EventsAppIdentityServer.Domain.Models;
using EventsAppIdentityServer.Infrastructure.Data;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EventsAppIdentityServer.Infrastructure.DbInitializer;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public DbInitializer(
        ApplicationDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
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

        var res = await _userManager.CreateAsync(admin, "Admin_123");

        await _userManager.AddToRoleAsync(admin, AppRoles.AdminRole);

        await _userManager.AddClaimsAsync(admin,
            [
                new Claim(JwtClaimTypes.Id, admin.Id),
                new Claim(JwtClaimTypes.Email, admin.Email),
                new Claim(JwtClaimTypes.Role, AppRoles.AdminRole)
            ]);


        AppUser user = new()
        {
            UserName = "ilya@gmail.com",
            Email = "ilya@gmail.com",
            EmailConfirmed = true,
            Name = "Ilya",
            Surname = "Rabets",
            Birthday = new DateTime(2004, 9, 16, 0, 0, 0, DateTimeKind.Utc),
        };

        await _userManager.CreateAsync(user, "Ilya_123");
        await _userManager.AddToRoleAsync(user, AppRoles.UserRole);

        //await _userManager.AddClaimsAsync(user,
        //    [
        //        new Claim(JwtClaimTypes.Id, user.Id),
        //        new Claim(JwtClaimTypes.Name, user.Name),
        //        new Claim("surname", user.Surname),
        //        new Claim(JwtClaimTypes.Email, user.Email),
        //        new Claim(JwtClaimTypes.BirthDate, user.Birthday.ToString()),
        //        new Claim(JwtClaimTypes.Role, AppRoles.UserRole)
        //    ]);

    }
}
