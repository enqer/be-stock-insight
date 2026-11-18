using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockInsight.Domain.Aggregates.Companies;
using StockInsight.Domain.Aggregates.Companies.Enums;

namespace StockInsight.Infrastructure.Persistence.Configurations;

internal sealed class StatementConfiguration : IEntityTypeConfiguration<Statement>
{
    public void Configure(EntityTypeBuilder<Statement> builder)
    {
        builder.HasKey(x => new { x.CompanyId, x.Period, x.FetchedAt });

        builder.HasDiscriminator<FinancialProfile>(nameof(FinancialProfile))
            .HasValue<StandardStatement>(FinancialProfile.Standard)
            .HasValue<BankStatement>(FinancialProfile.Bank);

        builder.HasOne<Company>()
            .WithMany(x => x.Statements)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
