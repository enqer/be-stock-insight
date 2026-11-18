using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;

namespace StockInsight.Infrastructure.Providers.Browser;

public sealed class PlaywrightBrowser(IHostEnvironment environment) : IAsyncDisposable
{
    private const int PageTimeoutMilliseconds = 60_000;

    private IPlaywright? _playwright;
    private IBrowser? _browser;

    public async Task<PlaywrightSession> OpenAsync(string url)
    {
        _browser ??= await LaunchChromiumAsync();
        var context = await _browser.NewContextAsync();
        try
        {
            var page = await context.NewPageAsync();
            page.SetDefaultTimeout(PageTimeoutMilliseconds);
            await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
            return new PlaywrightSession(context, page);
        }
        catch
        {
            await context.DisposeAsync();
            throw;
        }
    }

    private async Task<IBrowser> LaunchChromiumAsync()
    {
        if (environment.IsDevelopment())
        {
            await InstallChromiumAsync();
        }

        _playwright = await Playwright.CreateAsync();
        return await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
    }

    private static async Task InstallChromiumAsync()
    {
        var exitCode = await Task.Run(() => Microsoft.Playwright.Program.Main(["install", "chromium"]));
        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Playwright Chromium installation exited with code {exitCode}");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.DisposeAsync();
        }

        _playwright?.Dispose();
    }
}
