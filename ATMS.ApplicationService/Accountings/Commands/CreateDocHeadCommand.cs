
using ATMS.Domain.Contracts;
using ATMS.Domain.Entities;
using ErrorOr;
using MediatR;

namespace ATMS.ApplicationService;

public sealed record CreateDocHeadCommand(string DocSerialNo, string OfficeCode, string DocYear, string DocNo, string DocDesc) : IRequest<ErrorOr<DocHeadId>>;

public sealed class CreateDocHeadCommandHandler : IRequestHandler<CreateDocHeadCommand, ErrorOr<DocHeadId>>
{
    private readonly IDocHeadRepository _docHeadRepository;

    public CreateDocHeadCommandHandler(IDocHeadRepository docHeadRepository)
    {
        this._docHeadRepository = docHeadRepository;
    }

    public async Task<ErrorOr<DocHeadId>> Handle(CreateDocHeadCommand request, CancellationToken cancellationToken)
    {
        var entity = DocHead.Create(request.DocSerialNo, request.OfficeCode, request.DocYear, request.DocNo, request.DocDesc);
        var docHearId = await _docHeadRepository.AddDocHeadAsync(entity, cancellationToken);
        return docHearId;
    }
}