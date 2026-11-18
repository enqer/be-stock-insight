using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockInsight.Domain.Aggregates.Companies;
using StockInsight.Domain.Aggregates.Companies.Enums;
using StockInsight.Domain.Aggregates.Instruments;
using StockInsight.Domain.Aggregates.Instruments.Enums;
using StockInsight.Domain.Aggregates.Sectors;
using StockInsight.Domain.ValueObjects;
using StockInsight.Features.Common.Handlers;
using StockInsight.Infrastructure.Persistence;
using StockInsight.Infrastructure.Providers.Financial;
using StockInsight.Infrastructure.Providers.Financial.Models;
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
            var storedCompanyBySlug = await dbContext.Companies
                .Select(x => new
                {
                    x.Slug,
                    LatestPublishedAt = x.Statements.Where(y => y.PublishedAt <= y.FetchedAt).Max(y => y.PublishedAt),
                    HasSector = x.SectorId != null,
                })
                .ToDictionaryAsync(x => x.Slug, cancellationToken);

            var listedCompanies = await financialProvider.FetchCompaniesAsync();
            foreach (var listed in listedCompanies)
            {
                var isUpToDate = storedCompanyBySlug.TryGetValue(listed.Slug, out var stored)
                    && stored.HasSector
                    && stored.LatestPublishedAt >= listed.LastFinancialUpdate;
                if (isUpToDate)
                {
                    continue;
                }

                try
                {
                    await SyncCompanyAsync(listed.Slug, today, cancellationToken);
                }
                catch (Exception exception) when (exception is not OperationCanceledException)
                {
                    logger.LogError(exception, "Error occurred trying to sync financial data for: {Slug}", listed.Slug);
                }
                finally
                {
                    dbContext.ChangeTracker.Clear();
                }
            }
        }

        private async Task SyncCompanyAsync(string slug, DateOnly today, CancellationToken cancellationToken)
        {
            var details = await financialProvider.FetchCompanyAsync(slug);
            if (details.Warnings.Count > 0)
            {
                logger.LogWarning("Warnings occurred trying to sync financial data for: {Slug} with warnings: {Warnings}", slug, details.Warnings);
                return;
            }

            var company = await AddOrUpdateCompanyAsync(slug, details, cancellationToken);
            await AssignSectorAsync(company, details.Sector, slug, cancellationToken);
            company.AddStatements(StockWatchAdapter.ToStatements(company, details.Statements, today));
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<Company> AddOrUpdateCompanyAsync(string slug, CompanyDetailsDto details, CancellationToken cancellationToken)
        {
            var company = await dbContext.Companies.Include(x => x.Statements).SingleOrDefaultAsync(x => x.Slug == slug, cancellationToken);
            if (company is null)
            {
                company = Company.Create(slug, details.IsBank ? FinancialProfile.Bank : FinancialProfile.Standard);
                dbContext.Companies.Add(company);
                dbContext.Instruments.Add(Instrument.Stock(company.Id, new Ticker(details.Ticker), details.Name, details.Isin));
                return company;
            }

            var stock = await dbContext.Instruments.SingleAsync(x => x.CompanyId == company.Id && x.Kind == InstrumentKind.Stock, cancellationToken);
            stock.Rename(new Ticker(details.Ticker), details.Name);
            return company;
        }

        private async Task AssignSectorAsync(Company company, SectorDto? sector, string slug, CancellationToken cancellationToken)
        {
            if (sector is null)
            {
                logger.LogWarning("No sector on the provider page for: {Slug}", slug);
                return;
            }

            company.ChangeSector(await FindOrAddSectorAsync(sector, cancellationToken));
        }

        private async Task<Guid> FindOrAddSectorAsync(SectorDto source, CancellationToken cancellationToken)
        {
            var candidate = Sector.Create(source.MacroSector, source.Sector);
            var stored = await dbContext.Sectors.SingleOrDefaultAsync(x => x.MacroSector == candidate.MacroSector && x.Name == candidate.Name, cancellationToken);
            if (stored is not null)
            {
                return stored.Id;
            }

            dbContext.Sectors.Add(candidate);
            return candidate.Id;
        }
    }
}
