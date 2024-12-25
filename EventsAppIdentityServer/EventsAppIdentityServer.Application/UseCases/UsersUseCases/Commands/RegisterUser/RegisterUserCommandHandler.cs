using EventsAppIdentityServer.Domain.Entities;
using EventsAppIdentityServer.Domain.Models;
using EventsAppIdentityServer.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace EventsAppIdentityServer.Application.UseCases.UsersUseCases.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<AppUser>(request.UserDTO);

        var userByEmail = await _userManager.FindByEmailAsync(request.UserDTO.Email);

        if (userByEmail != null)
        {
            throw new AlreadyExistsException($"A user with the email '{request.UserDTO.Email}' already exists.");
        }

        var result1 = await _userManager.CreateAsync(user, request.UserDTO.Password);

        if (!result1.Succeeded)
        {
            var errors = string.Join(", ", result1.Errors);
            throw new Exception($"User is not successfully registered. Errors: {errors}");
        }

        var result2 = await _userManager.AddToRoleAsync(user, AppRoles.UserRole);

        if (!result2.Succeeded)
        {
            var errors = string.Join(", ", result2.Errors);
            throw new Exception($"User is not successfully added to role. Errors: {errors}");
        }
    }
}
