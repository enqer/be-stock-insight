using Microsoft.Extensions.DependencyInjection;
using StockInsight.Features.Common.Endpoints;
using StockInsight.Features.Markets;

namespace StockInsight.Features.Tests.Common;

public sealed class FeatureRegistrationTests
{
    private readonly IServiceCollection _services = new ServiceCollection().AddFeatures();

    [Fact]
    public void AddFeatures_registers_slice_endpoints() =>
        _services.ShouldContain(d => d.ServiceType == typeof(IEndpoint) && d.ImplementationType == typeof(GetMarkets.Endpoint));

    [Fact]
    public void AddFeatures_registers_slice_handlers_as_scoped() =>
        _services.ShouldContain(d => d.ServiceType == typeof(GetMarkets.Handler) && d.Lifetime == ServiceLifetime.Scoped);
}
