using ATMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ATMS.Infrastructure;

// DocItemConfiguration.cs
public class DocItemConfiguration : IEntityTypeConfiguration<DocItem>
{
    public void Configure(EntityTypeBuilder<DocItem> builder)
    {
        builder.ToTable("DocItems");

        builder.HasKey(e => e.Id);

        // Configure DocItemId
        builder.Property(e => e.Id)
            .HasColumnName("DocItemId")
            .HasConversion(
                v => v.Value,
                v => DocItemId.Create(v))
            .ValueGeneratedNever();

        // Configure DocHeadId with value converter
        builder.Property(e => e.DocHeadId)
            .HasColumnName("DocHeadId")
            .HasConversion(
                v => v.Value,
                v => DocHeadId.Create(v))
            .IsRequired();

        // Configure other properties
        builder.Property(e => e.DocSerialNo)
            .HasColumnName("DocSerNo")
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(e => e.ItemNo)
            .HasColumnName("ItemNo")
            .IsRequired();

        builder.Property(e => e.ItemDesc)
            .HasColumnName("ItemDesc")
            .HasMaxLength(500);

        // Configure navigation relationship
        builder.HasOne(e => e.DocHead)
            .WithMany(dh => dh.DocItems)
            .HasForeignKey(e => e.DocHeadId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
