using System;

public class Upgrade
{
    // 기획 데이터
    public readonly UpgradeSpecData SpecData;

    // 런타임 데이터
    public int Level { get; private set; }
    
    public Currency Cost => SpecData.BaseCost + Math.Pow(SpecData.CostMultiplier, Level);   // 지수 공식 : 기본 비용 + 증가량 ^ 레벨
    public double Damage => SpecData.BaseDamage + DamageIncreasement;      // 선형 공식 : 기본 비용 + 레벨 * 증가량
    public double DamageIncreasement => Level * SpecData.DamageMultiplier;
    public bool IsMaxLevel => Level >= SpecData.MaxLevel;
    
    public Upgrade(UpgradeSpecData specData)
    {
        if (specData.MaxLevel < 0) throw new System.ArgumentException("[Upgrade.cs] Max Level cannot be less than 0.");
        if (specData.BaseCost <= 0) throw new System.ArgumentException("[Upgrade.cs] Base Cost cannot be less than 0.");
        if (specData.BaseDamage <= 0) throw new System.ArgumentException("[Upgrade.cs] Base Damage cannot be less than 0.");
        if (specData.CostMultiplier <= 0) throw new System.ArgumentException("[Upgrade.cs] CostMultiplier cannot be less than 0.");
        if (specData.DamageMultiplier <= 0) throw new System.ArgumentException("[Upgrade.cs] DamageMultiplier cannot be less than 0.");
        if (string.IsNullOrEmpty(specData.Name)) throw new System.ArgumentException("[Upgrade.cs] Name cannot be null or empty.");
        if (string.IsNullOrEmpty(specData.Description)) throw new System.ArgumentException("[Upgrade.cs] Description cannot be null or empty.");
        SpecData = specData;
    }

    public bool TryLevelUp()
    {
        if (IsMaxLevel) return false;
        Level++;
        return true;
    }
}