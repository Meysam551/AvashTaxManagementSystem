
using ATMS.ApplicationService;
using ATMS.Domain.Contracts;
using ATMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ATMS.Infrastructure;

public static class InfrastructureConfigurationServiceInstaller
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopmentEnvironment = false)
    {
        var provider = configuration["Database:Provider"];

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            switch (provider)
            {
                case "SqlServer":
                    ConfigureSqlServer(options, configuration);
                    break;

                case "Oracle":
                    ConfigureOracle(options, configuration);
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Unsupported database provider: {provider}");
            }

            if (isDevelopmentEnvironment)
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
            switch (provider)
            {
                case "SqlServer":
                    ConfigureSqlServer(options, configuration);
                    break;

                case "Oracle":
                    ConfigureOracle(options, configuration);
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Unsupported database provider: {provider}");
            }
        }, lifetime: ServiceLifetime.Scoped);

        // Repositories
        services.AddScoped<IIdentityRepository, IdentityRepository>();

        //services.AddScoped<IDocHeadRepository, DocHeadRepository>();
        //services.AddScoped<IDocItemRepository, DocItemRepository>();
        services.AddScoped<IDocumentCoverRepository, DocumentCoverRepository>();
        services.AddScoped<IUserRepository, ATMSUserRepository>();

        //services.AddScoped<IEntityTypeConfiguration<DocHead>, DocHeadConfiguration>();

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    // -------------------------
    // SQL SERVER
    // -------------------------
    private static void ConfigureSqlServer(
        DbContextOptionsBuilder options,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("SqlServer")
            ?? BuildSqlServerConnectionString(configuration);

        options.UseSqlServer(connectionString, sql =>
        {
            sql.EnableRetryOnFailure();
        });
    }

    private static string BuildSqlServerConnectionString(IConfiguration config)
    {
        var server = config["SqlServer:Server"] ?? ".";
        var database = config["SqlServer:Database"] ?? "AppDb";
        var trustCert = config["SqlServer:TrustServerCertificate"] ?? "true";

        return
            $"Server={server};" +
            $"Database={database};" +
            $"Trusted_Connection=True;" +
            $"TrustServerCertificate={trustCert};";
    }

    // -------------------------
    // ORACLE
    // -------------------------
    private static void ConfigureOracle(
        DbContextOptionsBuilder options,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("Oracle")
            ?? BuildOracleConnectionString(configuration);

        options.UseOracle(connectionString);
    }

    private static string BuildOracleConnectionString(IConfiguration config)
    {
        var server = config["Oracle:Server"] ?? "localhost";
        var port = config["Oracle:Port"] ?? "1521";
        var service = config["Oracle:ServiceName"] ?? "XE";
        var userId = config["Oracle:UserId"] ?? "system";
        var password = config["Oracle:Password"] ?? "oracle";

        return
            $"User Id={userId};" +
            $"Password={password};" +
            $"Data Source=(DESCRIPTION=" +
            $"(ADDRESS=(PROTOCOL=TCP)(HOST={server})(PORT={port}))" +
            $"(CONNECT_DATA=(SERVICE_NAME={service})))";
    }
}
