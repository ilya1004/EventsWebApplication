using AutoMapper;
using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.Mapping;

public class ParticipantsMappingProfile : Profile
{
    public ParticipantsMappingProfile()
    {
        CreateMap<UserInfoDTO, Participant>()
            .ForMember(p => p.Person, opt =>
            opt.MapFrom(dto => new Person(dto.Name, dto.Surname, dto.Birthday)));
    }
}
