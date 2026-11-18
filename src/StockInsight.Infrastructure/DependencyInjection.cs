using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Npgsql;
using StockInsight.Infrastructure.Macro;
using StockInsight.Infrastructure.Persistence;
using StockInsight.Infrastructure.Persistence.Interceptors;
using StockInsight.Infrastructure.Providers.Browser;
using StockInsight.Infrastructure.Providers.Financial;
using StockInsight.Infrastructure.Seeds;

namespace StockInsight.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(AppDbContext.ConnectionName)!;

        var dataSource = NpgsqlDataSource.Create(connectionString);
        builder.Services.AddSingleton(_ => dataSource);
        builder.Services.AddDbContext<AppDbContext>((services, x) => x
            .UseNpgsql(dataSource)
            .AddInterceptors(new AuditableInterceptor(services.GetRequiredService<TimeProvider>())));
        builder.EnrichNpgsqlDbContext<AppDbContext>(x => x.DisableRetry = true);
        builder.Services.AddSingleton<ISqlRepository, SqlRepository>();

        builder.Services.AddHangfire(config => config
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseFilter(new AutomaticRetryAttribute { Attempts = 0 })
            .UseFilter(new DisableConcurrentExecutionAttribute(timeoutInSeconds: 0))
            .UsePostgreSqlStorage(
                options => options.UseNpgsqlConnection(connectionString),
                new PostgreSqlStorageOptions
                {
                    SchemaName = "hangfire",
                    UseSlidingInvisibilityTimeout = true,
                    DistributedLockTimeout = TimeSpan.FromHours(1),
                }));

        builder.Services.AddOptions<FinancialProviderOptions>()
            .BindConfiguration(FinancialProviderOptions.Section)
            .Validate(x => !string.IsNullOrWhiteSpace(x.CompaniesUrl) && !string.IsNullOrWhiteSpace(x.StatementUrl),
                $"missing StockWatch URLs in section '{FinancialProviderOptions.Section}'")
            .ValidateOnStart();
        builder.Services.AddScoped<PlaywrightBrowser>();
        builder.Services.AddScoped<IFinancialProvider, FinancialProvider>();
        builder.Services.AddOptions<HistoricalPricesSeedOptions>().BindConfiguration(HistoricalPricesSeedOptions.Section);
        builder.Services.AddScoped<HistoricalPricesSeed>();
        builder.Services.AddOptions<MacroIndicatorsSeedOptions>().BindConfiguration(MacroIndicatorsSeedOptions.Section);
        builder.Services.AddScoped<MacroIndicatorsSeed>();
        builder.Services.AddScoped<MacroIndicatorsImport>();
        builder.Services.TryAddSingleton(TimeProvider.System);

        return builder;
    }
}
