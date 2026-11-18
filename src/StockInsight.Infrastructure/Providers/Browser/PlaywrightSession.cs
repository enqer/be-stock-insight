using Microsoft.Playwright;

namespace StockInsight.Infrastructure.Providers.Browser;

public sealed class PlaywrightSession(IBrowserContext context, IPage page) : IAsyncDisposable
{
    public IPage Page { get; } = page;

    public ValueTask DisposeAsync() => context.DisposeAsync();
}
