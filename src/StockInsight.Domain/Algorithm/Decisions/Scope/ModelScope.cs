using StockInsight.Domain.Algorithm.Decisions.Scope.Models;

namespace StockInsight.Domain.Algorithm.Decisions.Scope;

public static class ModelScope
{
    private const string Insurance = "ubezpieczenia";
    private const string Brokerage = "maklerskie";
    private const string AssetManagement = "zarzadzanie aktywami";
    private const string InvestmentVehicles = "wehikuly inwestycyjne";
    private const string Receivables = "wierzytelnosci";

    public static IReadOnlyDictionary<string, ScopeExclusion> Exclusions { get; } =
        new Dictionary<string, ScopeExclusion>(StringComparer.Ordinal)
        {
            ["PZU"] = new(Insurance, "ubezpieczyciel; ROE 18,9% wobec naszego ROIC 13,2% — rozbieżność sama pokazuje, że IC nie opisuje tego biznesu"),
            ["EHG"] = new(Insurance, "Eurohold Bulgaria — grupa ubezpieczeniowa z dystrybucją samochodów"),

            ["IPE"] = new(Brokerage, "Ipopema Securities — dom maklerski"),
            ["XTB"] = new(Brokerage, "broker CFD/forex; kapitał obrotowy to depozyty klientów"),
            ["IDM"] = new(Brokerage, "IDM SA — dawny Dom Maklerski IDMSA"),

            ["QRS"] = new(AssetManagement, "Quercus TFI"),
            ["ALI"] = new(AssetManagement, "Altus — dawne Altus TFI"),
            ["SKH"] = new(AssetManagement, "Skarbiec Holding — TFI"),
            ["CSR"] = new(AssetManagement, "Caspar Asset Management"),

            ["MCI"] = new(InvestmentVehicles, "MCI Capital ASI — private equity"),
            ["JRH"] = new(InvestmentVehicles, "JR Holding ASI"),
            ["IMP"] = new(InvestmentVehicles, "Imperio ASI"),
            ["UNF"] = new(InvestmentVehicles, "Unfold.vc ASI"),
            ["ADV"] = new(InvestmentVehicles, "Adiuvo Investments — wehikuł inwestycyjny w biotechnologie"),
            ["SKL"] = new(InvestmentVehicles, "Skyline Investment"),
            ["NTC"] = new(InvestmentVehicles, "New Tech Capital"),
            ["RNC"] = new(InvestmentVehicles, "Reino Capital"),
            ["INC"] = new(InvestmentVehicles, "INC SA — bankowość inwestycyjna i portfel udziałów"),
            ["06N"] = new(InvestmentVehicles, "Magna Polonia — dawny NFI"),
            ["08N"] = new(InvestmentVehicles, "Octava — dawny NFI"),
            ["LRQ"] = new(InvestmentVehicles, "Larq — wehikuł inwestycyjny"),

            ["KRU"] = new(Receivables, "Kruk — zakup portfeli wierzytelności; IC to portfel, nie majątek"),
            ["BST"] = new(Receivables, "Best — zakup portfeli wierzytelności"),
            ["VIN"] = new(Receivables, "GPM Vindexus — zakup portfeli wierzytelności"),
            ["CAP"] = new(Receivables, "Capitea — dawny GetBack, portfele wierzytelności"),
            ["PRI"] = new(Receivables, "NPL Nova — portfele wierzytelności"),
            ["FFI"] = new(Receivables, "Fast Finance — portfele wierzytelności"),
            ["IBS"] = new(Receivables, "Wise Finance — działalność pożyczkowa"),
            ["MWT"] = new(Receivables, "M.W. Trade — finansowanie należności szpitalnych"),
        };
}
