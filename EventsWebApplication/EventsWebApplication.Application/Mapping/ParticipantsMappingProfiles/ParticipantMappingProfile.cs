﻿using AutoMapper;
using EventsWebApplication.Application.DTOs;

namespace EventsWebApplication.Application.Mapping.ParticipantsMappingProfiles;

public class ParticipantMappingProfile : Profile
{
    public ParticipantMappingProfile()
    {
        CreateMap<UserInfoDTO, Participant>()
            .ForMember(p => p.Person, opt =>
                opt.MapFrom(dto => new Person(dto.Name, dto.Surname, dto.Birthday)));
    }
}
