using System;

[Serializable]
public class UpgradeSaveData
{
    public UpgradeEntry[] Upgrades;

    public static UpgradeSaveData Default => new UpgradeSaveData
    {
        Upgrades = Array.Empty<UpgradeEntry>()
    };
}