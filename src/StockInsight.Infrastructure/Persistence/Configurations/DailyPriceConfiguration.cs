using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockInsight.Domain.Aggregates.DailyPrices;
using StockInsight.Domain.Aggregates.Instruments;

namespace StockInsight.Infrastructure.Persistence.Configurations;

internal sealed class DailyPriceConfiguration : IEntityTypeConfiguration<DailyPrice>
{
    public void Configure(EntityTypeBuilder<DailyPrice> builder)
    {
        builder.HasKey(x => new { x.InstrumentId, x.Date });

        builder.HasOne<Instrument>()
            .WithMany()
            .HasForeignKey(x => x.InstrumentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
