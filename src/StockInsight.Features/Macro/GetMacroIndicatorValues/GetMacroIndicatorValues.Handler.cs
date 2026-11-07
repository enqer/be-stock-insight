using StockInsight.Features.Common.Handlers;
using StockInsight.Features.Macro.GetMacroIndicatorValues.Models;
using StockInsight.Infrastructure.Persistence;

namespace StockInsight.Features.Macro.GetMacroIndicatorValues;

public static partial class GetMacroIndicatorValues
{
    internal sealed class Handler(ISqlRepository sql) : IHandler
    {
        public async Task<IReadOnlyList<Response>> HandleAsync(Guid indicatorId, CancellationToken cancellationToken)
        {
            var rows = await sql.QueryAsync<MacroValueRow>(MacroValueRow.Sql, new { IndicatorId = indicatorId }, cancellationToken);
            return rows.Select(x => new Response(x.Period, x.Value)).ToList();
        }
    }
}
