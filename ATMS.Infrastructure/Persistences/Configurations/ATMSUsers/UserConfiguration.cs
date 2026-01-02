
using ATMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ATMS.Infrastructure;

public class UserConfiguration : IEntityTypeConfiguration<ATMSUser>
{
    public void Configure(EntityTypeBuilder<ATMSUser> builder)
    {
        builder.ToTable("ATMSUsers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ATMSUserId(value));

        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.OwnsOne(x => x.Profile, profile =>
        {
            profile.Property(p => p.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            profile.Property(p => p.LastName)
                .HasMaxLength(100)
                .IsRequired();

            profile.OwnsOne(p => p.Email, email =>
            {
                email.Property(e => e.Value)
                     .HasColumnName("Email")
                     .HasMaxLength(200)
                     .IsRequired();
            });
        });
    }
}
