using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ATMS.Infrastructure;

// Infrastructure.Common/Interfaces/IDatabaseProvider.cs
public interface IDatabaseProvider
{
    DbContextOptionsBuilder ConfigureDbContext(DbContextOptionsBuilder options, string connectionString);
    void ConfigureServices(IServiceCollection services, string connectionString);
    bool CanHandle(string providerName);
}

// Infrastructure.Common/Interfaces/IMigrationService.cs
public interface IMigrationService
{
    Task MigrateAsync(CancellationToken cancellationToken = default);
    Task EnsureDatabaseCreatedAsync(CancellationToken cancellationToken = default);
}