using System.Reflection;
using Hangfire;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace StockInsight.Features.Common.Jobs;

internal static class JobExtensions
{
    private static readonly TimeZoneInfo _warsaw = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");

    private static readonly MethodInfo _schedule =
        typeof(JobExtensions).GetMethod(nameof(Schedule), BindingFlags.NonPublic | BindingFlags.Static)!;

    public static IServiceCollection AddJobs(this IServiceCollection services, Assembly assembly)
    {
        foreach (var jobType in JobTypes(assembly))
        {
            services.TryAddScoped(jobType);
        }

        return services;
    }

    public static IRecurringJobManager ScheduleJobs(this IRecurringJobManager manager, Assembly assembly)
    {
        foreach (var jobType in JobTypes(assembly))
        {
            _schedule.MakeGenericMethod(jobType).Invoke(null, [manager]);
        }

        return manager;
    }

    private static void Schedule<TJob>(IRecurringJobManager manager)
        where TJob : IJob =>
        manager.AddOrUpdate<TJob>(
            typeof(TJob).DeclaringType?.Name ?? typeof(TJob).Name,
            job => job.RunAsync(CancellationToken.None),
            TJob.Cron,
            new RecurringJobOptions { TimeZone = _warsaw });

    private static IEnumerable<Type> JobTypes(Assembly assembly) =>
        assembly.DefinedTypes.Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IJob)));
}
