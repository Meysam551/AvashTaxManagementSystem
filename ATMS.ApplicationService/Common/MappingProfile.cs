
using ATMS.Domain.Entities;
using ATMS.Shared.Dtos;
using AutoMapper;

namespace ATMS.ApplicationService;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DocumentCover, DocumentCoverDto>();
        CreateMap<DocumentCoverDto, DocumentCover>();
        CreateMap<List<DocumentCover>, IReadOnlyList<DocumentCoverDto>>()
            .ConvertUsing((src, dest, context) =>
                src.Select(x => context.Mapper.Map<DocumentCoverDto>(x)).ToList());
        CreateMap<DocumentCoverId, Guid>().ConvertUsing(id => id.Value);
    }
}
