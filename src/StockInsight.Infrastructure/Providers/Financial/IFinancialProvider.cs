using StockInsight.Infrastructure.Providers.Financial.Models;

namespace StockInsight.Infrastructure.Providers.Financial;

public interface IFinancialProvider
{
    Task<IReadOnlyList<CompanyDto>> FetchCompaniesAsync();
    Task<CompanyDetailsDto> FetchCompanyAsync(string slug);
}
