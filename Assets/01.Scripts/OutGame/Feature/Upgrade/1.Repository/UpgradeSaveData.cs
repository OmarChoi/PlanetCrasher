using System;
using Firebase.Firestore;
using UnityEngine;

[Serializable]
[FirestoreData]
public class UpgradeSaveData
{
    [SerializeField] private UpgradeEntry[] _upgrades;
    
    [FirestoreProperty]
    public UpgradeEntry[] Upgrades 
    {
        get => _upgrades;
        set => _upgrades = value;
    }
    
    public static UpgradeSaveData Default => new UpgradeSaveData
    {
        Upgrades = Array.Empty<UpgradeEntry>()
    };
}