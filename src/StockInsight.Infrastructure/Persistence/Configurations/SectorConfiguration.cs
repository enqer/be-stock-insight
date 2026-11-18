using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockInsight.Domain.Aggregates.Sectors;

namespace StockInsight.Infrastructure.Persistence.Configurations;

internal sealed class SectorConfiguration : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MacroSector).HasMaxLength(128);
        builder.Property(x => x.Name).HasMaxLength(128);
        builder.HasIndex(x => new { x.MacroSector, x.Name }).IsUnique();
    }
}
