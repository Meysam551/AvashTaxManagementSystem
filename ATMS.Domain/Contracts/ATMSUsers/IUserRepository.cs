
using ATMS.Domain.Entities;

namespace ATMS.Domain.Contracts;

public interface IUserRepository
{
    Task<ATMSUserId?> GetByIdAsync(ATMSUserId userId, CancellationToken ct);
    Task<ATMSUserId?> GetByUsernameAsync(string username, CancellationToken ct);
    Task AddAsync(ATMSUser user, CancellationToken ct);
    Task UpdateAsync(ATMSUser user, CancellationToken ct);
    Task<bool> ExistsAsync(ATMSUserId userId, CancellationToken ct);
}