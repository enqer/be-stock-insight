using System.Globalization;
using StockInsight.Domain.Algorithm.Calculations.Inputs.Statements.Models;
using StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks;

public static class SeriesQuality
{
    public const int MinPeriods = 8;

    public static SeriesQualityResult Evaluate(IReadOnlyList<Quarter> quarters)
    {
        var reasons = new List<string>();
        var quarantine = new HashSet<string>(StringComparer.Ordinal);
        var reports = PublicationSchedule.From(quarters);

        var offsetQuarantine = OffsetQuarantineRule.Apply(reports);
        if (offsetQuarantine.Reason is { } offsetReason)
        {
            quarantine.UnionWith(offsetQuarantine.QuarantinedPeriods);
            reasons.Add(offsetReason);
            reports = offsetQuarantine.Remaining;
            if (reports.Count < MinPeriods)
            {
                reasons.Add(string.Create(CultureInfo.InvariantCulture, $"po kwarantannie zostało {reports.Count} okresów"));
                return new SeriesQualityResult(Applicable: false, reasons, quarantine, ContinuousFromPeriod: null);
            }
        }

        var gapDays = PublicationSchedule.GapDays(reports);
        var cadence = CadenceRule.Check(gapDays);
        if (cadence.Rejection is { } irregular)
        {
            reasons.Add(irregular);
            return new SeriesQualityResult(Applicable: false, reasons, quarantine, ContinuousFromPeriod: null);
        }

        var firstGapQuarantine = FirstGapQuarantineRule.Apply(reports, gapDays, cadence.MedianGapDays!.Value);
        if (firstGapQuarantine is { QuarantinedPeriod: { } misdated, Reason: { } misdatedReason })
        {
            quarantine.Add(misdated);
            reasons.Add(misdatedReason);
            reports = firstGapQuarantine.Remaining;
            gapDays = PublicationSchedule.GapDays(reports);
            if (gapDays.Count == 0)
            {
                reasons.Add("po kwarantannie brak szeregu");
                return new SeriesQualityResult(Applicable: false, reasons, quarantine, ContinuousFromPeriod: null);
            }
        }

        var continuity = ContinuityRule.Apply(reports, gapDays);
        if (continuity.Reason is { } cut)
        {
            reasons.Add(cut);
        }

        return new SeriesQualityResult(continuity.Applicable, reasons, quarantine, continuity.ContinuousFromPeriod);
    }
}
