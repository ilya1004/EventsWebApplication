using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Commands.AddParticipantToEvent;
using EventsWebApplication.Domain.Abstractions.Data;
using EventsWebApplication.Domain.Abstractions.UserInfoProvider;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EventsWebApplication.Application.UseCases.UserUseCases.Queries.GetCurrentUserInfo;

public class GetCurrentUserInfoQueryHandler : IRequestHandler<GetCurrentUserInfoQuery, UserInfoDTO>
{
    private readonly IMapper _mapper;
    private readonly IUserInfoProvider _userInfoProvider;
    public GetCurrentUserInfoQueryHandler(IMapper mapper, IUserInfoProvider userInfoProvider)
    {
        _mapper = mapper;
        _userInfoProvider = userInfoProvider;
    }

    public async Task<UserInfoDTO> Handle(GetCurrentUserInfoQuery query, CancellationToken cancellationToken)
    {
        var result = await _userInfoProvider.GetUserInfoAsync(query.UserId, query.Token, cancellationToken);

        return _mapper.Map<UserInfoDTO>(result);
    }
}
