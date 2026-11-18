using Microsoft.Extensions.DependencyInjection;

namespace StockInsight.IntegrationTests;

public sealed class AppHostFixture : IAsyncLifetime
{
    private static readonly TimeSpan _startupTimeout = TimeSpan.FromMinutes(3);

    private DistributedApplication? _app;

    public HttpClient CreateApiClient() => _app!.CreateHttpClient("api");

    public async ValueTask InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.StockInsight_AppHost>();

        builder.Services.ConfigureHttpClientDefaults(http => http.AddStandardResilienceHandler());

        _app = await builder.BuildAsync();

        using var startupCancellation = new CancellationTokenSource(_startupTimeout);
        await _app.StartAsync(startupCancellation.Token);
        await _app.ResourceNotifications.WaitForResourceHealthyAsync("api", startupCancellation.Token);
    }

    public async ValueTask DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.DisposeAsync();
        }
    }
}
