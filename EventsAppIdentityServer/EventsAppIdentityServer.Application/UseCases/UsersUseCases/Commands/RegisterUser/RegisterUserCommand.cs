using EventsAppIdentityServer.Application.DTOs;
using MediatR;

namespace EventsAppIdentityServer.Application.UseCases.UsersUseCases.Commands.RegisterUser;

public sealed record RegisterUserCommand(RegisterUserDTO UserDTO) : IRequest;
