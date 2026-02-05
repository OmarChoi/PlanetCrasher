using System;

[Serializable]
public class EffectDescriptionEntry
{
    public EUpgradeEffectType Type;
    public string Format; // e.g., "Damage +{0}", "Spawn Rate +{0}%"
}
