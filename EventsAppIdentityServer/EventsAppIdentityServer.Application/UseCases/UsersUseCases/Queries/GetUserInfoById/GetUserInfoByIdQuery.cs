using EventsAppIdentityServer.Application.DTOs;
using MediatR;

namespace EventsAppIdentityServer.Application.UseCases.UsersUseCases.Queries.GetUserInfoById;

public sealed record GetUserInfoByIdQuery(string Id, string? AuthHeader) : IRequest<UserInfoDTO>;
