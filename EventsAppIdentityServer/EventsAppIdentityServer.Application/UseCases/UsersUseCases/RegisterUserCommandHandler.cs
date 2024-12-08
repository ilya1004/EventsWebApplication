using EventsAppIdentityServer.Domain.Entities;
using EventsAppIdentityServer.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EventsAppIdentityServer.Application.UseCases.UsersUseCases;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public RegisterUserCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            UserName = request.UserDTO.Email,
            Email = request.UserDTO.Email,
            Name = request.UserDTO.Name,
            Surname = request.UserDTO.Surname,
            EmailConfirmed = true,
            Birthday = new DateTime(request.UserDTO.Birthday, TimeOnly.MinValue, DateTimeKind.Utc),
        };

        var result1 = await _userManager.CreateAsync(user, request.UserDTO.Password);
        var result2 = await _userManager.AddToRoleAsync(user, AppRoles.UserRole);

        if (!result1.Succeeded)
        {
            var errors = string.Join(", ", result1.Errors, result2.Errors);
            throw new Exception($"User is not successfully registered. Errors: {errors}");
        }


    }
}
