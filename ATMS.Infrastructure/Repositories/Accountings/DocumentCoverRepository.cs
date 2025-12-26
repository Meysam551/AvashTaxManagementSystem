
using ATMS.Domain.Contracts;
using ATMS.Shared.Dtos;

namespace ATMS.Infrastructure;

public class DocumentCoverRepository : IDocumentCoverRepository
{
    public Task<Guid> AddAsync(DocumentCoverDto model, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
