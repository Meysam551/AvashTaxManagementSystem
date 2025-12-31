using ATMS.Domain.Entities;
using ATMS.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ATMS.Infrastructure;

public class MultiDatabaseUserManager : UserManager<User>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly IDatabaseSelector _databaseSelector;

    public MultiDatabaseUserManager(
        IUserStore<User> store,
        IOptions<IdentityOptions> options,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<User>> logger,
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IDatabaseSelector databaseSelector)
        : base(store, options, passwordHasher, userValidators,
              passwordValidators, keyNormalizer, errors, services, logger)
    {
        _dbContextFactory = dbContextFactory;
        _databaseSelector = databaseSelector;
    }

    public override async Task<User?> FindByEmailAsync(string email)
    {
        // Try SQL Server first
        var sqlServerDb = _dbContextFactory.CreateDbContext(DatabaseType.SqlServer);
        var sqlServerUser = await sqlServerDb.Set<User>()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (sqlServerUser != null)
        {
            sqlServerUser.DatabaseType = DatabaseType.SqlServer;
            return sqlServerUser;
        }

        // Try Oracle
        var oracleDb = _dbContextFactory.CreateDbContext(DatabaseType.Oracle);
        var oracleUser = await oracleDb.Set<User>()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (oracleUser != null)
        {
            oracleUser.DatabaseType = DatabaseType.Oracle;
        }

        return oracleUser;
    }

    public override async Task<User?> FindByNameAsync(string userName)
    {
        // Similar implementation as FindByEmailAsync
        // Search across both databases
        return await base.FindByNameAsync(userName);
    }
}
