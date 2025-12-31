using ATMS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore.Infrastructure;

namespace ATMS.Infrastructure.Data.Oracle;

public class ApplicationOracleDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationOracleDbContext(DbContextOptions<ApplicationOracleDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure for Oracle
        builder.Entity<User>(entity =>
        {
            entity.ToTable("USERS", "IDENTITY_SCHEMA");
            entity.Property(e => e.Id)
                .HasColumnName("USER_ID");
            entity.Property(e => e.UserName)
                .HasColumnName("USERNAME")
                .HasMaxLength(256);
            entity.Property(e => e.Email)
                .HasColumnName("EMAIL")
                .HasMaxLength(256);
            entity.Property(e => e.FirstName)
                .HasColumnName("FIRST_NAME")
                .HasMaxLength(100);
            entity.Property(e => e.LastName)
                .HasColumnName("LAST_NAME")
                .HasMaxLength(100);
            entity.Property(e => e.DatabaseType)
                .HasColumnName("DATABASE_TYPE")
                .HasConversion<string>()
                .HasMaxLength(20);

            // Oracle specific configurations
            entity.Property(e => e.ConcurrencyStamp)
                .HasColumnName("CONCURRENCY_STAMP");
        });

        builder.Entity<Role>(entity =>
        {
            entity.ToTable("ROLES", "IDENTITY_SCHEMA");
            entity.Property(e => e.Id)
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.Name)
                .HasColumnName("NAME")
                .HasMaxLength(256);
            entity.Property(e => e.Description)
                .HasColumnName("DESCRIPTION")
                .HasMaxLength(500);
        });

        // Oracle-specific: Adjust decimal precision
        builder.Entity<User>(entity =>
        {
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50); // Oracle has different length restrictions
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseOracle(
                options => options
                    .UseOracleSQLCompatibility("11"));
        }
    }
}