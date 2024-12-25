using AutoMapper;
using EventsAppIdentityServer.Application.DTOs;
using EventsAppIdentityServer.Application.Exceptions;
using EventsAppIdentityServer.Domain.Abstractions;
using EventsAppIdentityServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EventsAppIdentityServer.Application.UseCases.UsersUseCases.Queries.GetUserInfoById;

public class GetUserInfoByIdQueryHandler : IRequestHandler<GetUserInfoByIdQuery, UserInfoDTO>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ITokenValidationService _tokenValidationService;

    public GetUserInfoByIdQueryHandler(UserManager<AppUser> userManager, IMapper mapper, ITokenValidationService tokenValidationService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenValidationService = tokenValidationService;
    }

    public async Task<UserInfoDTO> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
    {
        _tokenValidationService.ValidateAuthHeader(request.AuthHeader, request.Id);

        var user = await _userManager.FindByIdAsync(request.Id);

        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.Id} not found");
        }

        var userInfoDTO = _mapper.Map<UserInfoDTO>(user);

        return userInfoDTO;
    }
}
