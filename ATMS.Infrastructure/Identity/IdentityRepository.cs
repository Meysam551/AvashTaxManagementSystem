
using ATMS.ApplicationService;
using ATMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ATMS.Infrastructure;

public class IdentityRepository : IIdentityRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public IdentityRepository(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ATMSUserId> CreateUserAsync(
        string username,
        string password,
        string email,
        CancellationToken ct)
    {
        var user = new ApplicationUser
        {
            UserName = username,
            Email = email,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                result.Errors.First().Description);

        return new ATMSUserId(user.Id);
    }

    public async Task UpdateEmailAsync(
        ATMSUserId userId,
        string email,
        CancellationToken ct)
    {
        var user = await FindUser(userId);
        user.Email = email;

        await _userManager.UpdateAsync(user);
    }

    public async Task AssignRoleAsync(
        ATMSUserId userId,
        string roleName,
        CancellationToken ct)
    {
        var user = await FindUser(userId);

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(
                new IdentityRole<Guid>(roleName));
        }

        await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<bool> IsInRoleAsync(
        ATMSUserId userId,
        string roleName,
        CancellationToken ct)
    {
        var user = await FindUser(userId);
        return await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task DeactivateAsync(
        ATMSUserId userId,
        CancellationToken ct)
    {
        var user = await FindUser(userId);
        user.IsActive = false;

        await _userManager.UpdateAsync(user);
    }

    // -------------------------
    // Private Helper
    // -------------------------
    private async Task<ApplicationUser> FindUser(ATMSUserId userId)
    {
        var user = await _userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null)
            throw new InvalidOperationException("Identity user not found");

        return user;
    }
}
