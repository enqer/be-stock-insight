using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockInsight.Domain.Aggregates.Companies;
using StockInsight.Domain.Aggregates.Companies.Enums;
using StockInsight.Domain.Aggregates.Instruments;
using StockInsight.Domain.Aggregates.Instruments.Enums;
using StockInsight.Domain.ValueObjects;
using StockInsight.Features.Common.Handlers;
using StockInsight.Infrastructure.Persistence;
using StockInsight.Infrastructure.Providers.Financial;
using StockInsight.Infrastructure.Providers.Financial.StockWatch;
using StockInsight.Infrastructure.Time;

namespace StockInsight.Features.Statements.SyncFinancialStatements;

public static partial class SyncFinancialStatements
{
    internal sealed class Handler(AppDbContext dbContext, IFinancialProvider financialProvider, TimeProvider time, ILogger<Handler> logger) : IHandler
    {
        public async Task HandleAsync(CancellationToken cancellationToken)
        {
            var today = MarketTime.Today(time);
            var latestReports = await dbContext.Companies
                .Select(x => new { x.Slug, Latest = x.Statements.Where(y => y.PublishedAt <= y.FetchedAt).Max(y => y.PublishedAt) })
                .ToDictionaryAsync(x => x.Slug, x => x.Latest, cancellationToken);

            var companies = await financialProvider.FetchCompaniesAsync();
            foreach (var entry in companies)
            {
                if (latestReports.GetValueOrDefault(entry.Slug) >= entry.LastFinancialUpdate)
                {
                    continue;
                }

                try
                {
                    await SyncAsync(entry.Slug, today, cancellationToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogError(ex, "Error occurred trying to sync financial data for: {Slug}", entry.Slug);
                }
                finally
                {
                    dbContext.ChangeTracker.Clear();
                }
            }
        }

        private async Task SyncAsync(string slug, DateOnly today, CancellationToken cancellationToken)
        {
            var details = await financialProvider.FetchCompanyAsync(slug);
            if (details.Warnings.Count > 0)
            {
                logger.LogWarning("Warnings occurred trying to sync financial data for: {Slug} with warnings: {Warnings}", slug, details.Warnings);
                return;
            }

            var company = await dbContext.Companies.Include(c => c.Statements).SingleOrDefaultAsync(x => x.Slug == slug, cancellationToken);
            if (company is null)
            {
                company = Company.Create(slug, details.IsBank ? FinancialProfile.Bank : FinancialProfile.Standard);
                dbContext.Companies.Add(company);
                dbContext.Instruments.Add(Instrument.Stock(company.Id, new Ticker(details.Ticker), details.Name, details.Isin));
            }
            else
            {
                var stock = await dbContext.Instruments.SingleAsync(x => x.CompanyId == company.Id && x.Kind == InstrumentKind.Stock, cancellationToken);
                stock.Rename(new Ticker(details.Ticker), details.Name);
            }

            company.AddStatements(StockWatchAdapter.ToStatements(company, details.Statements, today));
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
