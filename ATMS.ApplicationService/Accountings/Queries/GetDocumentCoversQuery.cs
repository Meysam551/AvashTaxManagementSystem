
using ATMS.Domain.Contracts;
using ATMS.Shared.Dtos;
using ATMS.Shared.Enums;
using ErrorOr;
using MediatR;

namespace ATMS.ApplicationService;

public sealed record GetDocumentCoversQuery : IRequest<ErrorOr<IReadOnlyList<DocumentCoverDto>>>
{
    public int? FiscalYear { get; set; }
    public DocumentType? DocumentType { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
}

public sealed class GetDocumentCoversQueryHandler : IRequestHandler<GetDocumentCoversQuery, ErrorOr<IReadOnlyList<DocumentCoverDto>>>
{
    private readonly IDocumentCoverRepository _documentCoverRepository;

    public GetDocumentCoversQueryHandler(IDocumentCoverRepository documentCoverRepository)
    {
        this._documentCoverRepository = documentCoverRepository;
    }

    public async Task<ErrorOr<IReadOnlyList<DocumentCoverDto>>> Handle(GetDocumentCoversQuery request, CancellationToken cancellationToken)
    {
        var result = await _documentCoverRepository.GetListAsync(cancellationToken);
        return ErrorOrFactory.From(result);
    }
}