using System;

public class Upgrade
{
    // 기획 데이터
    public readonly UpgradeMetaData MetaData;

    // 런타임 데이터
    public int Level { get; private set; }
    
    public Currency Cost => MetaData.BaseCost + Math.Pow(MetaData.CostMultiplier, Level);   // 지수 공식 : 기본 비용 + 증가량 ^ 레벨
    public double Damage => MetaData.BaseDamage + DamageIncreasement;      // 선형 공식 : 기본 비용 + 레벨 * 증가량
    public double DamageIncreasement => Level * MetaData.DamageMultiplier;
    public bool IsMaxLevel => Level >= MetaData.MaxLevel;
    
    public Upgrade(UpgradeMetaData metaData)
    {
        if (metaData.Icon == null) throw new NullReferenceException("[Upgrade.cs] Icon is null");
        if (metaData.MaxLevel < 0) throw new System.ArgumentException("[Upgrade.cs] Max Level cannot be less than 0.");
        if (metaData.BaseCost <= 0) throw new System.ArgumentException("[Upgrade.cs] Base Cost cannot be less than 0.");
        if (metaData.BaseDamage <= 0) throw new System.ArgumentException("[Upgrade.cs] Base Damage cannot be less than 0.");
        if (metaData.CostMultiplier <= 0) throw new System.ArgumentException("[Upgrade.cs] CostMultiplier cannot be less than 0.");
        if (metaData.DamageMultiplier <= 0) throw new System.ArgumentException("[Upgrade.cs] DamageMultiplier cannot be less than 0.");
        if (string.IsNullOrEmpty(metaData.Name)) throw new System.ArgumentException("[Upgrade.cs] Name cannot be null or empty.");
        if (string.IsNullOrEmpty(metaData.Description)) throw new System.ArgumentException("[Upgrade.cs] Description cannot be null or empty.");
        MetaData = metaData;
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
}