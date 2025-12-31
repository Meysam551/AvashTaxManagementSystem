using ATMS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ATMS.Infrastructure.Data.SqlServer;

public class ApplicationSqlServerDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationSqlServerDbContext(DbContextOptions<ApplicationSqlServerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure for SQL Server
        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.DatabaseType)
                .HasConversion<string>()
                .HasMaxLength(20);

            // SQL Server specific indexes
            entity.HasIndex(e => new { e.Email, e.DatabaseType })
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL");
        });

        builder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        // Configure relationships
        builder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles");
            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}