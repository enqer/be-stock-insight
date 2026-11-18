using Microsoft.EntityFrameworkCore;
using StockInsight.Domain.Aggregates.Companies;
using StockInsight.Domain.Aggregates.DailyPrices;
using StockInsight.Domain.Aggregates.DecisionRuns;
using StockInsight.Domain.Aggregates.Instruments;
using StockInsight.Domain.Aggregates.Macro;
using StockInsight.Domain.Aggregates.Portfolios;
using StockInsight.Domain.Aggregates.Sectors;

namespace StockInsight.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public const string ConnectionName = "stockinsight";

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Statement> Statements => Set<Statement>();
    public DbSet<Sector> Sectors => Set<Sector>();
    public DbSet<Instrument> Instruments => Set<Instrument>();
    public DbSet<DailyPrice> DailyPrices => Set<DailyPrice>();
    public DbSet<MacroIndicator> MacroIndicators => Set<MacroIndicator>();
    public DbSet<MacroIndicatorValue> MacroIndicatorValues => Set<MacroIndicatorValue>();
    public DbSet<Portfolio> Portfolios => Set<Portfolio>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<DecisionRun> DecisionRuns => Set<DecisionRun>();
    public DbSet<DecisionVerdict> DecisionVerdicts => Set<DecisionVerdict>();
    public DbSet<DecisionRankingEntry> DecisionRankingEntries => Set<DecisionRankingEntry>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSnakeCaseNamingConvention();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder.Properties<decimal>().HaveColumnType("numeric");

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
