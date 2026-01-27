using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private UpgradeContents _upgradeContents;

    private double _manualDamage = 1;
    private double _autoDamage = 0;
    private double _gold = 0;

    public double ManualDamage => _manualDamage;
    public double AutoDamage => _autoDamage;
    public double Gold => _gold;

    public event Action<double> OnGoldChanged;
    public event Action OnUpgradeCompleted;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    
    private void Start()
    {
        OnGoldChanged?.Invoke(_gold);
        OnUpgradeCompleted?.Invoke();
    }

    public bool TryUpgrade(UpgradeContentData contentData)
    {
        if (contentData == null) return false;
        if (_gold < contentData.CurrentPrice) return false;

        AddGold(-contentData.CurrentPrice);

        switch (contentData.BaseData.Type)
        {
            case EClickType.PerClick:
                _manualDamage += contentData.GetCurrentReward();
                break;
            case EClickType.AutoClick:
                _autoDamage += contentData.GetCurrentReward();
                break;
        }

        contentData.LevelUp();
        OnUpgradeCompleted?.Invoke();
        return true;
    }

    public void AddGold(double amount)
    {
        _gold += amount;
        OnGoldChanged?.Invoke(_gold);
    }
}
