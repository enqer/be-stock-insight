using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace StockInsight.Infrastructure.Seeds;

public static class SeedExtensions
{
    public static async Task SeedDevelopmentDataAsync(this IHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<HistoricalPricesSeed>().SeedAsync(CancellationToken.None);
        await scope.ServiceProvider.GetRequiredService<MacroIndicatorsSeed>().SeedAsync(CancellationToken.None);
    }
}
