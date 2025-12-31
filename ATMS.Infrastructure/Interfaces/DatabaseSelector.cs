using ATMS.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ATMS.Infrastructure;

public interface IDatabaseSelector
{
    DatabaseType GetDatabaseTypeForUser(string? username);
    string GetConnectionString(DatabaseType databaseType);
    DatabaseType DetermineDatabaseType();
}

public class DatabaseSelector : IDatabaseSelector
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DatabaseSelector(
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public DatabaseType GetDatabaseTypeForUser(string? username)
    {
        // Business logic to determine database for user
        // Example: Even emails go to SQL Server, odd to Oracle
        if (string.IsNullOrEmpty(username))
            return DetermineDatabaseType();

        return username.GetHashCode() % 2 == 0
            ? DatabaseType.SqlServer
            : DatabaseType.Oracle;
    }

    public string GetConnectionString(DatabaseType databaseType)
    {
        return databaseType switch
        {
            DatabaseType.SqlServer => _configuration.GetConnectionString("SqlServerConnection")!,
            DatabaseType.Oracle => _configuration.GetConnectionString("OracleConnection")!,
            _ => throw new ArgumentException($"Unsupported database type: {databaseType}")
        };
    }

    public DatabaseType DetermineDatabaseType()
    {
        // Read from cookie, header, or claim
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.Request.Cookies.TryGetValue("DatabaseType", out var dbTypeCookie) == true)
        {
            if (Enum.TryParse<DatabaseType>(dbTypeCookie, out var dbType))
                return dbType;
        }

        // Default fallback
        return DatabaseType.SqlServer;
    }
}