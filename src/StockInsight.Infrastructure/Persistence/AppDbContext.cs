using Microsoft.EntityFrameworkCore;

namespace StockInsight.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>Name of the database resource declared in the AppHost.</summary>
    public const string ConnectionName = "stockinsight";

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
