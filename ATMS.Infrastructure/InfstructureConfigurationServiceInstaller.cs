
using Microsoft.AspNetCore.Hosting;
using ATMS.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ATMS.Infrastructure;

public static class InfrastructureConfigurationServiceInstaller
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopmentEnvironment = false,
        string connectionStringName = "OracleConnection")
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);

        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = BuildOracleConnectionString(configuration);
        }

        // Register DbContextFactory (not regular DbContext)
        services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
            options.UseOracle(connectionString);

            if (isDevelopmentEnvironment)
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        },
        lifetime: ServiceLifetime.Scoped); // IMPORTANT: Make it Scoped

        // Register repositories - they'll use the factory
        services.AddScoped<IDocHeadRepository, DocHeadRepository>();
        services.AddScoped<IDocItemRepository, DocItemRepository>();
        services.AddScoped<IDocumentCoverRepository, DocumentCoverRepository>();

        return services;
    }

    private static string BuildOracleConnectionString(IConfiguration config)
    {
        var server = config["Oracle:Server"] ?? "localhost";
        var port = config["Oracle:Port"] ?? "1521";
        var service = config["Oracle:ServiceName"] ?? "XE";
        var userId = config["Oracle:UserId"] ?? "system";
        var password = config["Oracle:Password"] ?? "oracle";

        return $"User Id={userId};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={server})(PORT={port}))(CONNECT_DATA=(SERVICE_NAME={service})))";
    }
}
