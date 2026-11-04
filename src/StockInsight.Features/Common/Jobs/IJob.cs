namespace StockInsight.Features.Common.Jobs;

public interface IJob
{
    static abstract string Cron { get; }

    Task RunAsync(CancellationToken cancellationToken);
}
