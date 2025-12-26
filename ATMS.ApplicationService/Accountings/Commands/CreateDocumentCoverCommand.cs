
using ATMS.Domain.Contracts;
using ATMS.Domain.Entities;
using MediatR;

namespace ATMS.ApplicationService;

public record CreateDocumentCoverCommand(
    int FiscalYear,
    DateOnly DocumentDate,
    DocumentTypeEnum DocumentType,
    string Description) : IRequest<Guid>;

// Handler
public class CreateDocumentCoverCommandHandler
    : IRequestHandler<CreateDocumentCoverCommand, Guid>
{
    private readonly IDocumentCoverRepository _repository;

    public CreateDocumentCoverCommandHandler(IDocumentCoverRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(
        CreateDocumentCoverCommand request,
        CancellationToken cancellationToken)
    {
        var document = new Shared.Dtos.DocumentCoverDto
        {
            FiscalYear = request.FiscalYear,
            DocumentDate = request.DocumentDate
        };

        var result = await _repository.AddAsync(document, cancellationToken);

        return result;
    }
}
