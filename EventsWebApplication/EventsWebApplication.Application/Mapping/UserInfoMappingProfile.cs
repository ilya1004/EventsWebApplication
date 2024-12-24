using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Abstractions.UserInfoProvider;

namespace EventsWebApplication.Application.Mapping;

public class UserInfoMappingProfile : Profile
{
    public UserInfoMappingProfile()
    {
        CreateMap<UserInfoResponse, UserInfoDTO>();
    }
}
