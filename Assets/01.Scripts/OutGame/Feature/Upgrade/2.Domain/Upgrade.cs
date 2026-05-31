using System;

public class Upgrade
{
    // 기획 데이터
    public readonly UpgradeMetaData MetaData;

    // 런타임 데이터
    public int Level { get; private set; }

    public Currency Cost => IncreaseCalculator.CalculateCost
    (
        MetaData.CostIncreaseType,
        MetaData.BaseCost,
        MetaData.CostMultiplier,
        Level
    );

    public bool IsMaxLevel => Level >= MetaData.MaxLevel;

    // Level 0 = 미보유. 첫 구매(해금) 시 Level 1.
    public bool IsOwned => Level > 0;

    public Upgrade(UpgradeMetaData metaData, int level = 0)
    {
        if (level > metaData.MaxLevel) throw new ArgumentException("[Upgrade.cs] Level Data exceeds MaxLevel");
        if (metaData.Icon == null) throw new NullReferenceException("[Upgrade.cs] Icon is null");
        if (metaData.MaxLevel < 0) throw new ArgumentException("[Upgrade.cs] Max Level cannot be less than 0.");
        if (metaData.BaseCost <= 0) throw new ArgumentException("[Upgrade.cs] Base Cost must be greater than 0.");
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
        // Lv.1(Level 1)에서 Base(공식 index 0)가 적용되도록 한 칸 당긴다.
        // Level 0(미보유)도 index 0으로 평가되어 설명에 Lv.1 프리뷰가 표시된다.
        int effectLevel = Level > 0 ? Level - 1 : 0;
        UpgradeEffect[] effects = MetaData.Effects;
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].Type == effectType)
                return IncreaseCalculator.CalculateEffect(effects[i], effectLevel);
        }
        return 0;
    }
}
