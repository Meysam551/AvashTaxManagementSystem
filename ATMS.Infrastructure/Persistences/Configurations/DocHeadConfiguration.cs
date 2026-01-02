
using ATMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ATMS.Infrastructure;

public class DocHeadConfiguration : IEntityTypeConfiguration<DocHead>
{
    public void Configure(EntityTypeBuilder<DocHead> builder)
    {
        builder.ToTable("DocHeads");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.DocSerialNo).HasColumnName("DocSerNo")
            .HasMaxLength(15);
        builder.Property(e => e.OfficeCode).HasColumnName("OfficeCode")
            .HasMaxLength(20);
        builder.Property(e => e.DocYear).HasColumnName("DocYear")
            .HasMaxLength(4);
        builder.Property(e => e.DocNo).HasColumnName("DocNo")
            .HasMaxLength(10);
        builder.Property(e => e.DocDescription).HasColumnName("DocDesc")
            .HasMaxLength(100);

        builder.HasMany(e => e.DocItems)
            .WithOne(di => di.DocHead)
            .HasForeignKey(di => di.DocHeadId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
