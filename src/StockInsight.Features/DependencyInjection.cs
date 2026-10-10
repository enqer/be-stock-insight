using StockInsight.Features.Common.Endpoints;
using StockInsight.Features.Common.Handlers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace StockInsight.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.TryAddSingleton(TimeProvider.System);
        services.AddEndpoints(assembly);
        services.AddHandlers(assembly);
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        return services;
    }

    public static IEndpointRouteBuilder MapFeatures(this IEndpointRouteBuilder app) => app.MapEndpoints();
}
