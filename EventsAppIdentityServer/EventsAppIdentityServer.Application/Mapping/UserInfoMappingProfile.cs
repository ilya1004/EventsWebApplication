using AutoMapper;
using EventsAppIdentityServer.Application.DTOs;
using EventsAppIdentityServer.Domain.Entities;

namespace EventsAppIdentityServer.Application.Mapping;

public class UserInfoMappingProfile : Profile
{
    public UserInfoMappingProfile()
    {
        CreateMap<AppUser, UserInfoDTO>();
    }
}
