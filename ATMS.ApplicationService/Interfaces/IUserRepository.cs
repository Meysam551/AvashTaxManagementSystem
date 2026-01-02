
using ATMS.Domain.Entities;

namespace ATMS.ApplicationService;

public interface IUserRoleRepository
{
    Task<IReadOnlyList<string>> GetRolesAsync(ATMSUserId userId);
    Task<bool> IsInRoleAsync(ATMSUserId userId, string roleName);
}

