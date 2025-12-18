
using Microsoft.AspNetCore.Hosting;
using ATMS.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ATMS.Infrastructure;

public static class InfrastructureConfigurationServiceInstaller
{
    private static bool _isConfigured = false;
    private static readonly object _lockObject = new object();

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopmentEnvironment = false,  // تغییر به bool
        string connectionStringName = "OracleConnection")
    {
        if (_isConfigured)
            return services;

        lock (_lockObject)
        {
            if (_isConfigured)
                return services;

            var connectionString = configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = BuildOracleConnectionString(configuration);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseOracle(connectionString);

                if (isDevelopmentEnvironment) 
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            services.AddDbContextFactory<ApplicationDbContext>(options =>
            {
                options.UseOracle(connectionString);
            });


            services.AddScoped<IDocHeadRepository, DocHeadRepository>();
            services.AddScoped<IDocItemRepository, DocItemRepository>();

            _isConfigured = true;
        }

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
