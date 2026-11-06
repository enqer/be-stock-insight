using Hangfire;
using Hangfire.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace StockInsight.Features.Common.Jobs;

internal static class JobExtensions
{
    public static IServiceCollection AddJobs(this IServiceCollection services, Assembly assembly)
    {
        foreach (var jobType in assembly.ConcreteTypesOf<IJob>())
        {
            services.TryAddScoped(jobType);
        }

        return services;
    }

    public static IRecurringJobManager ScheduleJobs(this IRecurringJobManager manager, Assembly assembly, IConfiguration configuration)
    {
        foreach (var jobType in assembly.ConcreteTypesOf<IJob>())
        {
            var id = jobType.DeclaringType?.Name ?? jobType.Name;
            var cron = configuration[$"Jobs:{id}"]
                ?? throw new InvalidOperationException($"missing schedule for job '{id}' (configuration key 'Jobs:{id}')");
            var job = new Job(jobType, jobType.GetMethod(nameof(IJob.RunAsync))!, CancellationToken.None);
            manager.AddOrUpdate(id, job, cron, new RecurringJobOptions { TimeZone = MarketTime.Zone });
        }

        return manager;
    }
}
