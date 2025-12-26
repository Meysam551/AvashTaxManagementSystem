
using ATMS.Shared.Dtos;

namespace ATMS.Domain.Contracts;

public interface IDocumentCoverRepository
{
    Task<Guid> AddAsync(DocumentCoverDto model, CancellationToken cancellationToken);
}
