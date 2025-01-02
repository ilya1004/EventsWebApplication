using AutoMapper;
using EventsAppIdentityServer.Application.DTOs;
using EventsAppIdentityServer.Domain.Entities;
using MediatR;

namespace EventsAppIdentityServer.Application.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserDTO, AppUser>()
            .ForMember(u => u.UserName, opt =>
                opt.MapFrom(u => u.Email))
            .ForMember(u => u.EmailConfirmed, opt => 
                opt.MapFrom(_ => true))
            .ForMember(u => u.Birthday, opt =>
                opt.MapFrom(u => new DateTime(u.Birthday, TimeOnly.MinValue, DateTimeKind.Utc)));
    }
}
