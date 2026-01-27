
using ATMS.Shared;
using ATMS.Shared.Dtos;

namespace ATMS.Domain.Contracts;

public interface IDocumentCoverRepository
{
    Task<Result<Guid>> AddAsync(DocumentCoverDto model, CancellationToken cancellationToken);

    Task<IReadOnlyList<DocumentCoverDto>> GetListAsync(CancellationToken cancellationToken);
}
