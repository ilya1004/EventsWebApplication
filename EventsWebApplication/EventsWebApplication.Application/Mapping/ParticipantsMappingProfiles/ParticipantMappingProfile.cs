using AutoMapper;
using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.Mapping.ParticipantsMappingProfiles;

public class ParticipantMappingProfile : Profile
{
    public ParticipantMappingProfile()
    {
        CreateMap<UserInfoDTO, Participant>()
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => 0))
            .ForMember(dest => dest.Person, opt =>
                opt.MapFrom(src => new Person(src.Name, src.Surname, src.Birthday)))
            .ForMember(dest => dest.EventRegistrationDate, opt =>
                opt.MapFrom(src => DateTime.UtcNow));
    }
}
