using Microsoft.Extensions.DependencyInjection;

namespace StockInsight.IntegrationTests;

/// <summary>
/// Starts the whole AppHost (Postgres container + API) once per test assembly. Requires a running Docker.
/// </summary>
public sealed class AppHostFixture : IAsyncLifetime
{
    private static readonly TimeSpan StartupTimeout = TimeSpan.FromMinutes(3);

    private DistributedApplication? _app;

    public HttpClient CreateApiClient() => _app!.CreateHttpClient("api");

    public async ValueTask InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.StockInsight_AppHost>();

        builder.Services.ConfigureHttpClientDefaults(http => http.AddStandardResilienceHandler());

        _app = await builder.BuildAsync();

        using var cts = new CancellationTokenSource(StartupTimeout);
        await _app.StartAsync(cts.Token);
        await _app.ResourceNotifications.WaitForResourceHealthyAsync("api", cts.Token);
    }

    public async ValueTask DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.DisposeAsync();
        }
    }
}

[CollectionDefinition(Name)]
public sealed class AppHostCollection : ICollectionFixture<AppHostFixture>
{
    public const string Name = "AppHost";
}
