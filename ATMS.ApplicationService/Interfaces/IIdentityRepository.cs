
using ATMS.Domain.Entities;

namespace ATMS.ApplicationService;

public interface IIdentityRepository
{
    Task<ATMSUserId> CreateUserAsync(
        string username,
        string password,
        string email,
        CancellationToken ct);

    Task UpdateEmailAsync(
        ATMSUserId userId,
        string email,
        CancellationToken ct);

    Task AssignRoleAsync(
        ATMSUserId userId,
        string roleName,
        CancellationToken ct);

    Task<bool> IsInRoleAsync(
        ATMSUserId userId,
        string roleName,
        CancellationToken ct);

    Task DeactivateAsync(
        ATMSUserId userId,
        CancellationToken ct);
}

