using System;
using UnityEngine;

[Serializable]
public class UpgradeMetaData
{
    public EUpgradeType Type;
    public EClickType ClickType;
    public Sprite Icon;
    public int MaxLevel;
    public string Name;

    // Cost 설정
    public double BaseCost;
    public double CostMultiplier;
    public EIncreaseType CostIncreaseType;

    // 효과 배열 (복수 효과 지원)
    public UpgradeEffect[] Effects;
}
