using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    public static event Action OnDataChanged;

    private readonly Dictionary<EUpgradeType, Upgrade> _upgrades = new Dictionary<EUpgradeType, Upgrade>();
    [SerializeField] private UpgradeSpecTableSO _specTable;
    [SerializeField] private EffectDescriptionTableSO _effectDescriptionTable;

    private IUpgradeRepository _upgradeRepository;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // _upgradeRepository = new LocalUpgradeRepository(AccountManager.Instance.Email);
        _upgradeRepository = new FirebaseUpgradeRepository();
        OnDataChanged += SaveData;
        InitializeUpgrades().Forget();
    }
    
    private void OnDestroy()
    {
        OnDataChanged -= SaveData;
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private async UniTaskVoid InitializeUpgrades()
    {
        _upgrades.Clear();

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

            // 저장된 레벨이 있으면 사용, 없으면 0
            int level = savedLevels.GetValueOrDefault(metaData.Type, 0);
            _upgrades.Add(metaData.Type, new Upgrade(metaData, level));
        }
        
        OnDataChanged?.Invoke();
    }

    public Upgrade Get(EUpgradeType type) => _upgrades[type];

    public List<Upgrade> GetAll() => _upgrades.Values.ToList();

    public string GetDescription(Upgrade upgrade) => UpgradeDescriptionBuilder.GenerateAll(_effectDescriptionTable, upgrade);

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