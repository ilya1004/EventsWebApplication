using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Domain.Abstractions.BlobStorage;

namespace EventsWebApplication.Application.Mapping;

public class FileResponseMappingProfile : Profile
{
    public FileResponseMappingProfile()
    {
        CreateMap<FileResponse, FileResponseDTO>();
    }
}
