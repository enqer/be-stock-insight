using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockInsight.Domain.Aggregates.Companies;
using StockInsight.Domain.Aggregates.Instruments;
using StockInsight.Domain.Aggregates.Instruments.Enums;
using StockInsight.Domain.ValueObjects;

namespace StockInsight.Infrastructure.Persistence.Configurations;

internal sealed class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
{
    public void Configure(EntityTypeBuilder<Instrument> builder)
    {
        builder.ToTable(x => x.HasCheckConstraint(
            "ck_instruments_stock_has_company",
            $"(kind = {(int)InstrumentKind.Stock}) = (company_id IS NOT NULL)"));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Ticker)
            .HasConversion(x => x.Symbol, x => new Ticker(x))
            .HasMaxLength(16)
            .IsRequired();
        builder.HasIndex(x => x.Ticker).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Isin).HasMaxLength(12);

        builder.HasIndex(x => x.CompanyId).IsUnique().HasFilter("company_id IS NOT NULL");
        builder.HasOne<Company>()
            .WithMany()
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
