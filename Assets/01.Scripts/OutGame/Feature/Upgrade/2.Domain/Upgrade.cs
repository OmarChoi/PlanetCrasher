using System;
using System.Linq;

public class Upgrade
{
    // 기획 데이터
    public readonly UpgradeMetaData MetaData;

    // 런타임 데이터
    public int Level { get; private set; }

    public Currency Cost => UpgradeCalculator.CalculateCost
    (
        MetaData.CostIncreaseType,
        MetaData.BaseCost,
        MetaData.CostMultiplier,
        Level
    );

    public bool IsMaxLevel => Level >= MetaData.MaxLevel;

    public Upgrade(UpgradeMetaData metaData, int level = 0)
    {
        if (level > metaData.MaxLevel) throw new ArgumentOutOfRangeException("[Upgrade.cs] Level Data exceeds MaxLevel");
        if (metaData.Icon == null) throw new NullReferenceException("[Upgrade.cs] Icon is null");
        if (metaData.MaxLevel < 0) throw new ArgumentException("[Upgrade.cs] Max Level cannot be less than 0.");
        if (metaData.BaseCost <= 0) throw new ArgumentException("[Upgrade.cs] Base Cost cannot be less than 0.");
        if (metaData.CostMultiplier <= 0) throw new ArgumentException("[Upgrade.cs] CostMultiplier cannot be less than 0.");
        if (metaData.Effects == null || metaData.Effects.Length == 0) throw new ArgumentException("[Upgrade.cs] Effects cannot be null or empty.");
        if (string.IsNullOrEmpty(metaData.Name)) throw new ArgumentException("[Upgrade.cs] Name cannot be null or empty.");
        MetaData = metaData;
        Level = level;
    }

    public bool CanLevelUp()
    {
        return Level < MetaData.MaxLevel;
    }

    public bool TryLevelUp()
    {
        if (IsMaxLevel) return false;
        Level++;
        return true;
    }

    public double GetEffectValue(EUpgradeEffectType effectType)
    {
        var effect = MetaData.Effects.FirstOrDefault(e => e.Type == effectType);
        if (effect == null) return 0;
        return UpgradeCalculator.CalculateEffect(effect, Level);
    }
}
