using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using StockInsight.Domain.Aggregates.DailyPrices;
using StockInsight.Domain.Aggregates.Instruments;
using StockInsight.Domain.Aggregates.Instruments.Enums;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.ValueObjects;
using StockInsight.Infrastructure.Persistence;
using StockInsight.Infrastructure.Seeds.Models;

namespace StockInsight.Infrastructure.Seeds;

public sealed class HistoricalPricesSeed(
    IOptions<HistoricalPricesSeedOptions> options,
    AppDbContext dbContext,
    NpgsqlDataSource dataSource,
    ILogger<HistoricalPricesSeed> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await EnsureBetaIndexAsync(cancellationToken);

        var instrumentsWithoutPrices = await dbContext.Instruments.AsNoTracking()
            .Where(x => !dbContext.DailyPrices.Any(y => y.InstrumentId == x.Id))
            .ToListAsync(cancellationToken);

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        var imported = 0;
        foreach (var instrument in instrumentsWithoutPrices)
        {
            var directory = instrument.Kind == InstrumentKind.Index ? options.Value.IndicesDirectory : options.Value.StocksDirectory;
            var path = Path.Combine(directory, $"{instrument.Ticker.Symbol.ToLowerInvariant()}.txt");
            if (!File.Exists(path))
            {
                logger.LogWarning("No historical prices file for {Ticker}: {Path}", instrument.Ticker, path);
                continue;
            }

            await CopyPricesAsync(connection, ReadPriceFile(instrument.Id, path), cancellationToken);
            imported++;
        }

        logger.LogInformation("Historical prices imported for {Imported} of {Pending} instruments without prices", imported, instrumentsWithoutPrices.Count);
    }

    private async Task EnsureBetaIndexAsync(CancellationToken cancellationToken)
    {
        var ticker = new Ticker(MarketConstants.BetaIndexTicker);
        if (!await dbContext.Instruments.AnyAsync(x => x.Ticker == ticker, cancellationToken))
        {
            dbContext.Instruments.Add(Instrument.Index(ticker, "mWIG40TR"));
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    internal static IEnumerable<DailyPrice> ReadPriceFile(Guid instrumentId, string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        foreach (var row in csv.GetRecords<StooqRowDto>())
        {
            yield return DailyPrice.Create(instrumentId, row.Date, row.Close, row.Volume);
        }
    }

    internal static async Task CopyPricesAsync(NpgsqlConnection connection, IEnumerable<DailyPrice> prices, CancellationToken cancellationToken)
    {
        await using var copy = await connection.BeginBinaryImportAsync(
            "COPY daily_prices (instrument_id, date, close, volume) FROM STDIN (FORMAT BINARY)", cancellationToken);
        foreach (var price in prices)
        {
            await copy.StartRowAsync(cancellationToken);
            await copy.WriteAsync(price.InstrumentId, NpgsqlDbType.Uuid, cancellationToken);
            await copy.WriteAsync(price.Date, NpgsqlDbType.Date, cancellationToken);
            await copy.WriteAsync(price.Close, NpgsqlDbType.Double, cancellationToken);
            await copy.WriteAsync(price.Volume, NpgsqlDbType.Double, cancellationToken);
        }

        await copy.CompleteAsync(cancellationToken);
    }
}
