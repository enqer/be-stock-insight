namespace StockInsight.Features.Common.Jobs;

public interface IJob
{
    Task RunAsync(CancellationToken cancellationToken);
}
