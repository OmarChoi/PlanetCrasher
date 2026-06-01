using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public static event Action OnDataChanged;
    public static event Action OnDataInitialized;

    protected override bool IsPersistent => true;

    private readonly Dictionary<EUpgradeType, Upgrade> _upgrades = new Dictionary<EUpgradeType, Upgrade>();
    private readonly List<Upgrade> _upgradeList = new List<Upgrade>();
    [SerializeField] private UpgradeSpecTableSO _specTable;

    public bool IsInitialized { get; private set; }

    private IUpgradeRepository _upgradeRepository;
    protected override void Initialize()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        _upgradeRepository = new FirebaseUpgradeRepository();
#else
        _upgradeRepository = new MockUpgradeRepository();
#endif
        InitializeUpgrades().Forget();
        OnDataChanged += SaveData;
    }

    protected override void Cleanup()
    {
        OnDataChanged -= SaveData;
    }

    private async UniTaskVoid InitializeUpgrades()
    {
        _upgrades.Clear();
        _upgradeList.Clear();

        UpgradeMetaData[] upgradeDatas = _specTable.UpgradeSpecDatas;

        UpgradeSaveData saveData = await _upgradeRepository.Load();

        // Null 체크 및 기본값 처리
        if (saveData?.Upgrades == null)
        {
            saveData = UpgradeSaveData.Default;
        }

        // Dictionary 용량 미리 지정 (리사이징 비용 절감)
        Dictionary<EUpgradeType, int> savedLevels = new Dictionary<EUpgradeType, int>(saveData.Upgrades.Length);
        foreach (UpgradeEntry entry in saveData.Upgrades)
        {
            if (entry == null) continue;
            savedLevels[entry.Type] = entry.Level;
        }

        foreach (UpgradeMetaData metaData in upgradeDatas)
        {
            if (_upgrades.ContainsKey(metaData.Type))
            {
                throw new InvalidOperationException
                (
                    $"[UpgradeManager.cs] Duplicate upgrade type: {metaData.Type}"
                );
            }

            // 저장된 레벨이 있으면 사용, 없으면 시작 보유 레벨(StartLevel)
            int level = savedLevels.GetValueOrDefault(metaData.Type, metaData.StartLevel);
            Upgrade upgrade = new Upgrade(metaData, level);
            _upgrades.Add(metaData.Type, upgrade);
            _upgradeList.Add(upgrade);
        }

        await UniTask.Yield();

        IsInitialized = true;
        OnDataInitialized?.Invoke();
    }

    public Upgrade Get(EUpgradeType type) => _upgrades[type];

    public IReadOnlyList<Upgrade> GetAll() => _upgradeList;
    
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
        List<UpgradeEntry> upgrades = new List<UpgradeEntry>(_upgrades.Count);

        foreach (KeyValuePair<EUpgradeType, Upgrade> kvp in _upgrades)
        {
            if (kvp.Value.Level >= 0)
            {
                upgrades.Add(new UpgradeEntry
                {
                    Type = kvp.Key,
                    Level = kvp.Value.Level
                });
            }
        }

        UpgradeSaveData data = new UpgradeSaveData
        {
            Upgrades = upgrades.ToArray()
        };
        _upgradeRepository.Save(data);
    }
    #endregion Save/Load

}