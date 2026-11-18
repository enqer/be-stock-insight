using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockInsight.Domain.Aggregates.Macro;

namespace StockInsight.Infrastructure.Persistence.Configurations;

internal sealed class MacroIndicatorConfiguration : IEntityTypeConfiguration<MacroIndicator>
{
    public void Configure(EntityTypeBuilder<MacroIndicator> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Country).HasMaxLength(2).IsRequired();
        builder.Property(x => x.Code).HasMaxLength(32).IsRequired();
        builder.HasIndex(x => new { x.Country, x.Code }).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(256);
    }
}
