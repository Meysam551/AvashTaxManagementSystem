
using ATMS.Infrastructure.Data.Oracle;
using ATMS.Infrastructure.Data.SqlServer;
using ATMS.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ATMS.Infrastructure.Persistences;

public interface IDbContextFactory
{
    DbContext CreateDbContext(DatabaseType databaseType);
    DbContext CreateDbContextForUser(string? username);
}

public class DbContextFactory : IDbContextFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDatabaseSelector _databaseSelector;

    public DbContextFactory(
        IServiceProvider serviceProvider,
        IDatabaseSelector databaseSelector)
    {
        _serviceProvider = serviceProvider;
        _databaseSelector = databaseSelector;
    }

    public DbContext CreateDbContext(DatabaseType databaseType)
    {
        var connectionString = _databaseSelector.GetConnectionString(databaseType);

        return databaseType switch
        {
            DatabaseType.SqlServer => new ApplicationSqlServerDbContext(
                new DbContextOptionsBuilder<ApplicationSqlServerDbContext>()
                    .UseSqlServer(connectionString)
                    .Options),
            DatabaseType.Oracle => new ApplicationOracleDbContext(
                new DbContextOptionsBuilder<ApplicationOracleDbContext>()
                    .UseOracle(connectionString)
                    .Options),
            _ => throw new ArgumentException($"Unsupported database type: {databaseType}")
        };
    }

    public DbContext CreateDbContextForUser(string? username)
    {
        var databaseType = _databaseSelector.GetDatabaseTypeForUser(username);
        return CreateDbContext(databaseType);
    }
}