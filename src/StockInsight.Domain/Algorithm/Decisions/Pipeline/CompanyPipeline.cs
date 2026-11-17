using StockInsight.Domain.Algorithm.Calculations.Inputs.Market;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Models;
using StockInsight.Domain.Algorithm.Decisions.Pipeline.Steps;

namespace StockInsight.Domain.Algorithm.Decisions.Pipeline;

public static class CompanyPipeline
{
    public const int MinUniversePeriods = 8;

    public static CompanyOutcome Evaluate(string ticker, Func<CompanyInputs> loadInputs, PriceSeries indexPrices, RiskFreeRate riskFreeRate)
    {
        if (ScopeStep.Run(ticker).Rejection is { } outOfScope)
        {
            return Reject(ticker, outOfScope);
        }

        var inputs = loadInputs();
        if (SourceLengthStep.Run(inputs).Rejection is { } tooShort)
        {
            return Reject(ticker, tooShort);
        }

        var quality = SeriesQualityStep.Run(inputs.Quarters);
        if (quality.Rejection is { } notApplicable)
        {
            return Reject(ticker, notApplicable);
        }

        var series = BuildSeries(inputs.Quarters, inputs.Prices, indexPrices, riskFreeRate);
        series = QuarantineStep.Run(series, quality.Value);
        series = ContinuityCutStep.Run(series, quality.Value);

        var enough = MinDecisionsStep.Run(series);
        return enough.Rejection is { } tooFew
            ? Reject(ticker, tooFew)
            : new AcceptedCompany(ticker, enough.Value, inputs.Prices);
    }

    public static IReadOnlyList<DecisionPoint> BuildSeries(IReadOnlyList<Quarter> quarters, PriceSeries prices, PriceSeries indexPrices, RiskFreeRate riskFreeRate)
    {
        var prepared = PrepareStep.Run(quarters);
        var periods = PeriodMeasuresStep.Run(prepared);
        var periodsByPublication = PublicationOrderStep.Run(periods);
        return DecisionMeasuresStep.Run(prepared, periodsByPublication, prices, indexPrices, riskFreeRate);
    }

    private static RejectedCompany Reject(string ticker, StepRejection rejection) => new(ticker, rejection.Stage, rejection.Reason);
}
