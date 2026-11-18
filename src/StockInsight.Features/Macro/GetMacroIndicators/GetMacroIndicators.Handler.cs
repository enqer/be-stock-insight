using StockInsight.Features.Common.Handlers;
using StockInsight.Features.Macro.GetMacroIndicators.Models;
using StockInsight.Infrastructure.Persistence;

namespace StockInsight.Features.Macro.GetMacroIndicators;

public static partial class GetMacroIndicators
{
    internal sealed class Handler(ISqlRepository sql) : IHandler
    {
        public async Task<IReadOnlyList<Response>> HandleAsync(string country, CancellationToken cancellationToken)
        {
            var rows = await sql.QueryAsync<MacroIndicatorRow>(MacroIndicatorRow.Sql, new { Country = country.Trim().ToUpperInvariant() }, cancellationToken);
            return rows.GroupBy(x => x.Id).Select(ToResponse).ToList();
        }
    }

    internal static Response ToResponse(IGrouping<Guid, MacroIndicatorRow> rows)
    {
        var indicator = rows.First();
        var values = rows.Where(x => x.Period is not null).Select(x => (Period: x.Period!.Value, Value: x.Value!.Value)).ToList();
        if (values.Count == 0)
        {
            return new Response(
                indicator.Id, indicator.Country, indicator.Code, indicator.Name, indicator.Kind, indicator.Unit,
                Period: null, Value: null, PreviousValue: null, RecentValues: []);
        }

        var latest = values[^1];
        double? previousValue = values.Count > 1 ? values[^2].Value : null;
        var sameMonthLastYear = new DateOnly(latest.Period.Year - 1, latest.Period.Month, 1);
        var lastYearValues = values.Where(x => x.Period >= sameMonthLastYear).Select(x => x.Value).ToList();
        return new Response(
            indicator.Id, indicator.Country, indicator.Code, indicator.Name, indicator.Kind, indicator.Unit,
            latest.Period, latest.Value, previousValue, lastYearValues);
    }
}
