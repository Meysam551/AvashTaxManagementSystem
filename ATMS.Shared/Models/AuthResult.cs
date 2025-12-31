
namespace ATMS.Shared;

public class AuthResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? TokenExpiration { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public IList<string>? Roles { get; set; }
    public List<string> Errors { get; set; } = new();

    private AuthResult(bool success, string? token = null, string? refreshToken = null,
        DateTime? tokenExpiration = null, string? userId = null, string? userName = null,
        IList<string>? roles = null)
    {
        Success = success;
        Token = token;
        RefreshToken = refreshToken;
        TokenExpiration = tokenExpiration;
        UserId = userId;
        UserName = userName;
        Roles = roles;
    }

    public static AuthResult SuccessResult(string token, string refreshToken,
        DateTime? tokenExpiration = null, string? userId = null, string? userName = null,
        IList<string>? roles = null)
    {
        return new AuthResult(true, token, refreshToken, tokenExpiration, userId, userName, roles);
    }

    public static AuthResult Failure(params string[] errors)
    {
        var result = new AuthResult(false);
        result.Errors.AddRange(errors);
        return result;
    }

    public static AuthResult Failure(List<string> errors)
    {
        var result = new AuthResult(false);
        result.Errors.AddRange(errors);
        return result;
    }
}