using System.Globalization;
using System.Text.RegularExpressions;
using StockInsight.Domain.Aggregates.Companies;
using StockInsight.Domain.Aggregates.Companies.Enums;
using StockInsight.Infrastructure.Providers.Financial.Models;

namespace StockInsight.Infrastructure.Providers.Financial.StockWatch;

public static partial class StockWatchAdapter
{
    private const string LongTermLiabilities = "Zobowiązania długoterminowe";
    private const string ShortTermLiabilities = "Zobowiązania krótkoterminowe";

    public static IReadOnlyList<Statement> ToStatements(Company company, IReadOnlyList<StatementDto> quarters, DateOnly fetchedAt) =>
        company.FinancialProfile == FinancialProfile.Standard
            ? [.. quarters.Select(x => ToStandardStatement(company.Id, x, fetchedAt))]
            : [.. quarters.Select(x => ToBankStatement(company.Id, x, fetchedAt))];

    private static StandardStatement ToStandardStatement(Guid companyId, StatementDto quarter, DateOnly fetchedAt)
    {
        var cells = quarter.Items;
        var longTerm = cells
            .TakeWhile(x => x.Label != ShortTermLiabilities)
            .SkipWhile(x => x.Label != LongTermLiabilities)
            .Skip(1)
            .ToList();
        var shortTerm = cells
            .SkipWhile(x => x.Label != ShortTermLiabilities)
            .Skip(1)
            .ToList();

        double? Number(string label) => NumberOf(cells, label);
        double? LongTerm(string label) => NumberOf(longTerm, label, LongTermLiabilities);
        double? ShortTerm(string label) => NumberOf(shortTerm, label, ShortTermLiabilities);

        var nonCurrentAssets = Number("Aktywa trwałe");
        var currentAssets = Number("Aktywa obrotowe");
        var totalAssets = StockWatchNumber.Parse(cells.FirstOrDefault(x => x.Label == "Suma bilansowa")?.Value)
            ?? nonCurrentAssets + currentAssets;

        return StandardStatement.Create(
            companyId,
            QuarterEnd(quarter.Period),
            fetchedAt,
            PublicationDateOf(cells),
            revenue: ZeroAsMissing(Number("Przychody")),
            grossProfitOnSales: Number("Zysk brutto na sprzedaży"),
            sellingExpenses: Number("Koszty sprzedaży"),
            generalAndAdministrativeExpenses: Number("Koszty ogólnego zarządu"),
            profitOnSales: Number("Zysk ze sprzedaży"),
            otherOperatingIncome: Number("Pozostałe przychody operacyjne"),
            otherOperatingExpenses: Number("Pozostałe koszty operacyjne"),
            ebit: Number("Zysk operacyjny"),
            financialIncome: Number("Przychody Finansowe"),
            financialExpenses: Number("Koszty finansowe"),
            profitBeforeTax: Number("Zysk brutto"),
            discontinuedOperations: Number("Zysk z działalności zaniechanej"),
            netProfit: Number("Zysk netto"),
            ebitda: Number("EBITDA"),
            operatingCashFlow: Number("Przepływy operacyjne"),
            depreciation: Number("Amortyzacja"),
            investingCashFlow: Number("Przepływy inwestycyjne"),
            capex: Number("Capex"),
            financingCashFlow: Number("Przepływy finansowe"),
            leasePayments: Number("Leasingi"),
            dividends: Number("Dywidenda"),
            totalCashFlow: Number("Przepływy razem"),
            nonCurrentAssets: ZeroAsMissing(nonCurrentAssets),
            currentAssets: ZeroAsMissing(currentAssets),
            inventories: Number("Zapasy"),
            tradeReceivables: Number("Należności handlowe"),
            otherCurrentAssets: Number("Pozostałe aktywa obrotowe"),
            shortTermInvestments: Number("Inwestycje krótkoterminowe"),
            cash: Number("w tym gotówka"),
            totalAssets: ZeroAsMissing(totalAssets),
            equity: ZeroAsMissing(Number("Kapitał własny")),
            minorityInterests: Number("Udziały niekontrolujące"),
            nonCurrentLiabilities: Number(LongTermLiabilities),
            longTermBorrowings: LongTerm("Kredyty i pożyczki"),
            longTermLeaseLiabilities: LongTerm("Z tytułu leasingu"),
            longTermTradePayables: LongTerm("Z tytułu dostaw i usług"),
            longTermBonds: LongTerm("Z tytułu emisji papierów wartościowych"),
            otherLongTermLiabilities: LongTerm("Inne zobowiązania długoterminowe"),
            currentLiabilities: Number(ShortTermLiabilities),
            shortTermBorrowings: ShortTerm("Kredyty i pożyczki"),
            shortTermLeaseLiabilities: ShortTerm("Z tytułu leasingu"),
            shortTermTradePayables: ShortTerm("Z tytułu dostaw i usług"),
            shortTermBonds: ShortTerm("Z tytułu emisji papierów wartościowych"),
            otherShortTermLiabilities: ShortTerm("Inne zobowiazania krótkoterminowe"),
            shares: Number("Liczba akcji"),
            shareCapital: Number("Kapitał podstawowy"),
            shareIssue: Number("Emisja akcji"));
    }

    private static BankStatement ToBankStatement(Guid companyId, StatementDto quarter, DateOnly fetchedAt)
    {
        var cells = quarter.Items;
        double? Number(string label) => NumberOf(cells, label);

        return BankStatement.Create(
            companyId,
            QuarterEnd(quarter.Period),
            fetchedAt,
            PublicationDateOf(cells),
            interestIncome: Number("Przychody z tytułu odsetek"),
            interestExpense: -Number("Koszty z tytułu odsetek"),
            netInterestIncome: Number("Wynik z tytyłu odsetek"),
            netFeeIncome: Number("Wynik z tytułu prowizji i opłat"),
            dividendIncome: Number("Przychody z tytułu dywidend"),
            tradingResult: Number("Wynik handlowy i rewaluacja"),
            otherFinancialInstrumentsResult: Number("Wynik na pozostałych instrumentach finansowych"),
            otherOperatingIncome: Number("Pozostałe przychody operacyjne"),
            otherOperatingExpenses: Number("Pozostałe koszty operacyjne"),
            administrativeExpenses: Number("Koszty administracyjne"),
            loanImpairment: Number("Odpisy z tytułu utraty wartości kredytów"),
            operatingProfit: Number("Zysk operacyjny"),
            shareOfAssociatesProfit: Number("Udział w zyskach jednostek podporządkowanych"),
            profitBeforeTax: Number("Zysk brutto"),
            discontinuedOperations: Number("Zysk z działalności zaniechanej"),
            netProfit: Number("Zysk netto"),
            operatingCashFlow: Number("Przepływy z działalności operacyjnej"),
            depreciation: Number("Amortyzacja"),
            capex: Number("Capex"),
            dividends: Number("Dywidenda"),
            shareIssue: Number("Emisja Akcji"),
            customerLoans: Number("Należności od klientów"),
            totalAssets: ZeroAsMissing(Number("Aktywa razem")),
            customerDeposits: Number("Zobowiązania wobec klientów"),
            equity: ZeroAsMissing(Number("Kapitał własny")),
            minorityInterests: Number("Udziały niekontrolujące"),
            shares: Number("Liczba akcji"));
    }

    private static double? ZeroAsMissing(double? value) => value == 0 ? null : value;

    private static double? NumberOf(IReadOnlyList<StatementCellDto> cells, string label, string? section = null) =>
        StockWatchNumber.Parse(CellOf(cells, label, section).Value);

    private static StatementCellDto CellOf(IReadOnlyList<StatementCellDto> cells, string label, string? section = null) =>
        cells.FirstOrDefault(x => x.Label == label) ?? throw new MissingLabelException(label, section);

    private static DateOnly? PublicationDateOf(IReadOnlyList<StatementCellDto> cells) =>
        DateOnly.TryParseExact(CellOf(cells, "Data raportu").Value?.Trim(), "d.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : null;

    private static DateOnly QuarterEnd(string period)
    {
        var match = QuarterRegex().Match(period.Trim());
        if (!match.Success)
        {
            throw new InvalidDataException($"period '{period}' is not a quarter");
        }

        var month = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture) * 3;
        var year = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        return new DateOnly(year, month, DateTime.DaysInMonth(year, month));
    }

    [GeneratedRegex("^Q([1-4]) +([1-9][0-9]{3})$")]
    private static partial Regex QuarterRegex();
}
