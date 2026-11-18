using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockInsight.Domain.Aggregates.Macro;

namespace StockInsight.Infrastructure.Persistence.Configurations;

internal sealed class MacroIndicatorValueConfiguration : IEntityTypeConfiguration<MacroIndicatorValue>
{
    public void Configure(EntityTypeBuilder<MacroIndicatorValue> builder)
    {
        builder.HasKey(x => new { x.IndicatorId, x.Period, x.FetchedAt });

        builder.HasOne<MacroIndicator>()
            .WithMany()
            .HasForeignKey(x => x.IndicatorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
