using Microsoft.Extensions.Hosting;
using StockInsight.Infrastructure.Persistence;

namespace StockInsight.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<AppDbContext>(AppDbContext.ConnectionName);

        return builder;
    }
}
