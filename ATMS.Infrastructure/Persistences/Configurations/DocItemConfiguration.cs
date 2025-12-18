
using ATMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ATMS.Infrastructure;

public class DocItemConfiguration : IEntityTypeConfiguration<DocItem>
{
    public void Configure(EntityTypeBuilder<DocItem> builder)
    {
        builder.ToTable("DocHeads");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.DocSerialNo).HasColumnName("DocSerNo")
            .HasMaxLength(15);
        builder.Property(e => e.ItemNo).HasColumnName("ItemNo").HasDefaultValue(1);
        builder.Property(e => e.ItemDesc).HasColumnName("ItemDesc")
            .HasMaxLength(100);
    }
}
