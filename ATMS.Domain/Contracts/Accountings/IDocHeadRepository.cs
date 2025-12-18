
using ATMS.Domain.Entities;

namespace ATMS.Domain.Contracts;

public interface IDocHeadRepository
{
    Task<DocHeadId> AddDocHeadAsync(DocHead model, CancellationToken cancellationToken);

    Task<IReadOnlyList<DocHead>> GetListAsync(CancellationToken cancellationToken);
}