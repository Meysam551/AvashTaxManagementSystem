
using ATMS.ApplicationService.Dtos;
using ATMS.Domain.Contracts;
using AutoMapper;
using ErrorOr;
using MediatR;

namespace ATMS.ApplicationService;

public sealed record GetDocHeadListQuery() : IRequest<ErrorOr<IReadOnlyList<DocHeadDto>>>;

public sealed class GetDocHeadListQueryHandler : IRequestHandler<GetDocHeadListQuery, ErrorOr<IReadOnlyList<DocHeadDto>>>
{
    private readonly IDocHeadRepository _docHeadRepository;
    private readonly IMapper _mapper;

    public GetDocHeadListQueryHandler(IDocHeadRepository docHeadRepository, IMapper mapper)
    {
        this._docHeadRepository = docHeadRepository;
        this._mapper = mapper;
    }

    async Task<ErrorOr<IReadOnlyList<DocHeadDto>>> IRequestHandler<GetDocHeadListQuery, ErrorOr<IReadOnlyList<DocHeadDto>>>.Handle(GetDocHeadListQuery request, CancellationToken cancellationToken)
    {
        var result = await _docHeadRepository.GetListAsync(cancellationToken).ConfigureAwait(false);
        if (result is null) return Error.NotFound($"موردی یافت نشد");
        var dtos = _mapper.Map<IReadOnlyList<DocHeadDto>>(result);
        return ErrorOrFactory.From(dtos);
    }
}