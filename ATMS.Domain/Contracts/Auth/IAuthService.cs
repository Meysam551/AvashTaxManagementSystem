
using ATMS.Shared;

namespace ATMS.Domain.Contracts;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password, string ipAddress);
    Task<AuthResult> RefreshTokenAsync(string token, string ipAddress);
    Task<bool> RevokeTokenAsync(string token, string ipAddress);
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    Task<string> GeneratePasswordResetTokenAsync(string email);
    Task<bool> IsEmailConfirmedAsync(Guid userId);
}
