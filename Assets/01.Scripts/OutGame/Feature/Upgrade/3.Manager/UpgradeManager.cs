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

    private IUpgradeRepository _upgradeRepository;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _upgradeRepository = new LocalUpgradeRepository();
        InitializeUpgrades();
        OnDataChanged += SaveData;
    }
    
    private void InitializeUpgrades()
    {
        int[] levels = _upgradeRepository.Load().Levels;
        UpgradeMetaData[] upgradeDatas = _specTable.UpgradeSpecDatas;
    
        if (levels.Length != upgradeDatas.Length)
        {
            throw new InvalidOperationException
            (
                $"[UpgradeManager.cs] Mismatch between saved levels ({levels.Length}) and upgrade specs ({upgradeDatas.Length})"
            );
        }
        for (int i = 0; i < upgradeDatas.Length; i++)
        {
            UpgradeMetaData upgrade = upgradeDatas[i];
        
            if (_upgrades.ContainsKey(upgrade.Type))
            {
                throw new InvalidOperationException
                (
                    $"[UpgradeManager.cs] Duplicate upgrade type: {upgrade.Type}"
                );
            }
        
            _upgrades.Add(upgrade.Type, new Upgrade(upgrade, levels[i]));
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

    #region Save/Load
    private void SaveData()
    {
        UpgradeSaveData data = new UpgradeSaveData
        {
            Levels = new int[_upgrades.Count]
        };

        for (int i = 0; i < _upgrades.Count; i++)
        {
            data.Levels[i] = _upgrades[(EUpgradeType)i].Level;
        }
        _upgradeRepository.Save(data);
    }

    #endregion Save/Load
}