namespace StockInsight.Domain.Algorithm.Decisions.SeriesChecks.Models;

public sealed record ScheduledReport(DateOnly PublicationDate, string PeriodLabel, DateOnly PeriodEnd);
