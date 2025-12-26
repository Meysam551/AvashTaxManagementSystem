
using ATMS.Domain.Entities;
using ATMS.Shared.Dtos;
using AutoMapper;

namespace ATMS.ApplicationService;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DocumentCover, DocumentCoverDto>();
        CreateMap<DocumentCoverId, Guid>().ConvertUsing(id => id.Value);
    }
}
