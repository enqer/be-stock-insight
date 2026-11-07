using StockInsight.Features.Common.Jobs;

namespace StockInsight.Features.Statements.SyncFinancialStatements;

public static partial class SyncFinancialStatements
{
    internal sealed class Job(Handler handler) : IJob
    {
        public Task RunAsync(CancellationToken cancellationToken) => handler.HandleAsync(cancellationToken);
    }
}
