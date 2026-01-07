
using ATMS.Domain.Contracts;
using ATMS.Shared.Enums;
using MediatR;

namespace ATMS.ApplicationService;

public record CreateDocumentCoverCommand(
    int FiscalYear,
    DateOnly DocumentDate,
    DocumentType DocumentType,
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
            DocumentDate = request.DocumentDate,
            Description = request.Description,
            DocumentNumber = 1,
            DocumentType = request.DocumentType,
            PostedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _repository.AddAsync(document, cancellationToken);

        return result.Value;
    }
}
