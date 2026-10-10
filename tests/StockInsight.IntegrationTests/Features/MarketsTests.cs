namespace StockInsight.IntegrationTests.Features;

[Collection(AppHostCollection.Name)]
public sealed class MarketsTests(AppHostFixture fixture)
{
    private sealed record MarketDto(int Id, string Name);

    [Fact]
    public async Task Get_markets_returns_all_supported_markets()
    {
        var ct = TestContext.Current.CancellationToken;
        using var client = fixture.CreateApiClient();

        var markets = await client.GetFromJsonAsync<List<MarketDto>>("/markets", ct);

        markets.ShouldNotBeNull();
        markets.Select(m => m.Name).ShouldContain("GPW");
    }

    [Fact]
    public async Task Health_endpoint_reports_healthy()
    {
        var ct = TestContext.Current.CancellationToken;
        using var client = fixture.CreateApiClient();

        var response = await client.GetAsync("/health", ct);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
