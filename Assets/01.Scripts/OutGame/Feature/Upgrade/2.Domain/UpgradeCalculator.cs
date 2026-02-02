using System;

public static class UpgradeCalculator
{
    private static double Calculate(EIncreaseType type, double baseValue, double multiplier, int level)
    {
        return type switch
        {
            EIncreaseType.Linear => baseValue + (level * multiplier),
            EIncreaseType.Exponential => baseValue * Math.Pow(multiplier, level),
            EIncreaseType.ExponentialAdditive => baseValue + Math.Pow(multiplier, level),
            _ => baseValue
        };
    }
    
    public static Currency CalculateCost(EIncreaseType type, double baseCost, double multiplier, int level) => Calculate(type, baseCost, multiplier, level);
    public static double CalculateEffect(UpgradeEffect effect, int level) => Calculate(effect.IncreaseType, effect.BaseValue, effect.Multiplier, level);
}
