
using ATMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ATMS.Infrastructure;

public class DocumentCoverConfiguration : IEntityTypeConfiguration<DocumentCover>
{
    public void Configure(EntityTypeBuilder<DocumentCover> builder)
    {
        builder.ToTable("DocumentCovers");
        builder.HasKey(e => e.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new DocumentCoverId(value));
    }
}