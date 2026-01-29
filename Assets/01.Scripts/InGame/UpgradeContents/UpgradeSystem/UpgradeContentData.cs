using UnityEngine;

public class UpgradeContentData
{
    public UpgradeData BaseData { get; private set; }
    public int CurrentLevel { get; private set; }
    public double CurrentPrice { get; private set; }
    
    public UpgradeContentData(UpgradeData baseData)
    {
        BaseData = baseData;
        CurrentLevel = 0;
        CurrentPrice = baseData.Price;
    }

    public void LevelUp()
    {
        CurrentLevel++;
        CalculateNextPrice();
    }

    private void CalculateNextPrice()
    {
        CurrentPrice = BaseData.Price * Mathf.Pow(BaseData.PriceMultiplier, CurrentLevel);
    }

    public int GetCurrentReward()
    {
        return BaseData.RewardAmount;
    }
    
    public string GetDescription()
    {
        return BaseData.Description;
    }
}