using EventsAppIdentityServer.Application.DTOs;
using MediatR;

namespace EventsAppIdentityServer.Application.UseCases.UsersUseCases;

public sealed record RegisterUserCommand(RegisterUserDTO UserDTO) : IRequest;
