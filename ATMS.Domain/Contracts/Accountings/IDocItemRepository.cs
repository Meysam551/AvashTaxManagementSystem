
using ATMS.Domain.Entities;

namespace ATMS.Domain.Contracts;

public interface IDocItemRepository
{
    Task<DocItemId> AddDDooccItemAsync(DocItem model);
}
