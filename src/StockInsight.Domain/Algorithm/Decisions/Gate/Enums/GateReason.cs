namespace StockInsight.Domain.Algorithm.Decisions.Gate.Enums;

public enum GateReason
{
    Passed = 1,
    NoWacc = 2,
    Direction = 3,
    RoicNotAboveWacc = 4,
    RoiicBelowWacc = 5,
    Dilution = 6,
    EarningsAnomaly = 7,
    OutsideUniverse = 8,
}
