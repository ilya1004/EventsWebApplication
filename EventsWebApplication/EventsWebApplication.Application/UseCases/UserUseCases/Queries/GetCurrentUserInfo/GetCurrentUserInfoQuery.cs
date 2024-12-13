using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.UseCases.UserUseCases.Queries.GetCurrentUserInfo;

public sealed record GetCurrentUserInfoQuery(string UserId, string Token) : IRequest<UserInfoDTO>;
