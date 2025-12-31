using ATMS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ATMS.Shared;
using Microsoft.AspNetCore.Authentication;

namespace ATMS.Infrastructure;

public class MultiDatabaseSignInManager : SignInManager<User>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly IDatabaseSelector _databaseSelector;

    public MultiDatabaseSignInManager(
        UserManager<User> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<User> claimsFactory,
        IOptions<IdentityOptions> options,
        ILogger<SignInManager<User>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<User> confirmation,
        IDbContextFactory dbContextFactory,
        IDatabaseSelector databaseSelector)
        : base(userManager, contextAccessor, claimsFactory, options, logger, schemes, confirmation)
    {
        _dbContextFactory = dbContextFactory;
        _databaseSelector = databaseSelector;
    }

    public override async Task<SignInResult> PasswordSignInAsync(
        string userName,
        string password,
        bool isPersistent,
        bool lockoutOnFailure)
    {
        // Determine which database to use for this user
        var databaseType = _databaseSelector.GetDatabaseTypeForUser(userName);

        // Get the appropriate DbContext
        var dbContext = _dbContextFactory.CreateDbContext(databaseType);

        // Find user in the specific database
        var user = await dbContext.Set<User>()
            .FirstOrDefaultAsync(u => u.UserName == userName || u.Email == userName);

        if (user == null)
        {
            return SignInResult.Failed;
        }

        // Set database type on user
        user.DatabaseType = databaseType;

        // Verify password using UserManager
        var result = await UserManager.CheckPasswordAsync(user, password);

        if (result)
        {
            // Store database type in cookie
            var httpContext = ContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Response.Cookies.Append(
                    "DatabaseType",
                    databaseType.ToString(),
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
            }

            await SignInAsync(user, isPersistent);
            return SignInResult.Success;
        }

        return SignInResult.Failed;
    }
}
