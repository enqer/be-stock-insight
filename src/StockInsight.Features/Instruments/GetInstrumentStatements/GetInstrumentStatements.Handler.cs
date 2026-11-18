using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StockInsight.Domain.Aggregates.Companies;
using StockInsight.Domain.ValueObjects;
using StockInsight.Features.Common.Handlers;
using StockInsight.Infrastructure.Persistence;

namespace StockInsight.Features.Instruments.GetInstrumentStatements;

public static partial class GetInstrumentStatements
{
    internal sealed class Handler(AppDbContext dbContext) : IHandler
    {
        public async Task<IReadOnlyList<Response>> HandleAsync(string ticker, CancellationToken cancellationToken)
        {
            var symbol = new Ticker(ticker);
            var statements = await dbContext.Statements.AsNoTracking()
                .Where(x => x.PublishedAt == null || x.PublishedAt <= x.FetchedAt)
                .Where(x => dbContext.Instruments.Any(y => y.CompanyId == x.CompanyId && y.Ticker == symbol))
                .ToListAsync(cancellationToken);

            return
            [
                .. statements
                    .GroupBy(x => x.Period)
                    .Select(x => x.MaxBy(y => y.FetchedAt)!)
                    .OrderByDescending(x => x.Period)
                    .Select(ToResponse),
            ];
        }
    }

    internal static Response ToResponse(Statement statement)
    {
        var filledItems = statement.GetType().GetProperties()
            .Where(x => x.PropertyType == typeof(double?))
            .Select(x => (Name: JsonNamingPolicy.CamelCase.ConvertName(x.Name), Value: (double?)x.GetValue(statement)))
            .Where(x => x.Value is not null)
            .ToDictionary(x => x.Name, x => x.Value!.Value);
        return new Response(statement.Period, statement.PublishedAt, filledItems);
    }
}
