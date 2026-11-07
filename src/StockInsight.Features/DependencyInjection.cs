using StockInsight.Features.Common.Endpoints;
using StockInsight.Features.Common.Handlers;
using StockInsight.Features.Common.Jobs;
using StockInsight.Features.Decisions.Common;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace StockInsight.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddEndpoints(assembly);
        services.AddHandlers(assembly);
        services.AddJobs(assembly);
        services.TryAddScoped<DecisionUniverseLoader>();

        return services;
    }

    public static IEndpointRouteBuilder MapFeatures(this IEndpointRouteBuilder app) => app.MapEndpoints();

    public static IRecurringJobManager ScheduleFeatureJobs(this IRecurringJobManager manager, IConfiguration configuration) =>
        manager.ScheduleJobs(typeof(DependencyInjection).Assembly, configuration);
}
