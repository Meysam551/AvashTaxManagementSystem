
using ATMS.ApplicationService;
using ATMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ATMS.Infrastructure;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRoleRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync(ATMSUserId userId)
    {
        var user = await _userManager.FindByIdAsync(userId.Value.ToString());
        var result = await _userManager.GetRolesAsync(user!);
        return result.AsReadOnly();
    }

    public async Task<bool> IsInRoleAsync(ATMSUserId userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.Value.ToString());
        return await _userManager.IsInRoleAsync(user!, roleName);
    }
}

