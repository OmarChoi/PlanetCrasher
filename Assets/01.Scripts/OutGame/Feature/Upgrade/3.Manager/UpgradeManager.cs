using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    public static event Action OnDataChanged;

    private readonly Dictionary<EUpgradeType, Upgrade> _upgrades = new Dictionary<EUpgradeType, Upgrade>();
    [SerializeField] private UpgradeSpecTableSO _specTable;

    private void Awake()
    {
        Instance = this;
        InitializeUpgrades();
    }

    private void InitializeUpgrades()
    {
        foreach (UpgradeMetaData upgrade in _specTable.UpgradeSpecDatas)
        {
            if (_upgrades.ContainsKey(upgrade.Type))
            {
                throw new Exception("[UpgradeManager] Duplicate upgrade type: " + upgrade.Type);
            }
            _upgrades.Add(upgrade.Type, new Upgrade(upgrade));
        }
        OnDataChanged?.Invoke();
    }

    public Upgrade Get(EUpgradeType type) => _upgrades[type] ?? null;
    
    public List<Upgrade> GetAll() => _upgrades.Values.ToList();
    
    public bool CanLevelUp(EUpgradeType type)
    {
        if (!_upgrades.TryGetValue(type, out Upgrade upgrade)) return false;
        if (upgrade.IsMaxLevel) return false;
        return CurrencyManager.Instance.CanAfford(ECurrencyType.Gold, upgrade.Cost);
    }

    public bool TryLevelUp(EUpgradeType type)
    {
        if (!_upgrades.TryGetValue(type, out Upgrade upgrade)) return false;
        if (!CurrencyManager.Instance.TrySpend(ECurrencyType.Gold, upgrade.Cost)) return false;
        if (!upgrade.TryLevelUp()) return false;
        OnDataChanged?.Invoke();
        return true;
    }
}