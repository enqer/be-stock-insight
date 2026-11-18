using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockInsight.Domain.Aggregates.DecisionRuns;
using StockInsight.Domain.Aggregates.Portfolios;

namespace StockInsight.Infrastructure.Persistence.Configurations;

internal sealed class DecisionRunConfiguration : IEntityTypeConfiguration<DecisionRun>
{
    public void Configure(EntityTypeBuilder<DecisionRun> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasIndex(x => new { x.PortfolioId, x.DecisionDate }).IsUnique();

        builder.HasOne<Portfolio>()
            .WithMany()
            .HasForeignKey(x => x.PortfolioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
