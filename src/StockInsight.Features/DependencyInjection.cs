using Microsoft.Extensions.DependencyInjection;

namespace StockInsight.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        return services;
    }
}