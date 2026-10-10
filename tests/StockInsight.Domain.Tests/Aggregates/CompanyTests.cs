using StockInsight.Domain.Aggregates.Company;
using StockInsight.Domain.Enums;
using StockInsight.Domain.ValueObjects;

namespace StockInsight.Domain.Tests.Aggregates;

public sealed class CompanyTests
{
    [Fact]
    public void Create_sets_properties_and_marks_company_active()
    {
        var company = Company.Create(new Ticker("CDR"), "CD Projekt", "PLOPTTC00011", Market.GPW);

        company.Ticker.ShouldBe(new Ticker("CDR"));
        company.Name.ShouldBe("CD Projekt");
        company.ISIN.ShouldBe("PLOPTTC00011");
        company.Market.ShouldBe(Market.GPW);
        company.IsActive.ShouldBeTrue();
        company.Prices.ShouldBeEmpty();
    }
}
