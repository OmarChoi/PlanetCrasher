using System;
using UnityEngine;

[Serializable]
public class UpgradeMetaData
{
    public EUpgradeType Type;
    public EClickType ClickType;
    public Sprite Icon;
    public int MaxLevel;
    public double BaseCost;
    public double BaseDamage;
    public double CostMultiplier;
    public double DamageMultiplier;
    public string Name;
    public string Description;
}