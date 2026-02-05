using UnityEngine;

public abstract class UpgradeContent : MonoBehaviour
{
    protected abstract EUpgradeType UpgradeType { get; }

    private Upgrade _cachedCachedUpgrade;
    protected Upgrade CachedUpgrade => _cachedCachedUpgrade;

    protected double GetEffectValue(EUpgradeEffectType type) => _cachedCachedUpgrade.GetEffectValue(type);

    private void Awake()
    {
        UpgradeManager.OnDataInitialized += InitializeUpgradeData;
        Init();
    }

    protected virtual void Init() { }
    
    private void OnUpgradeChanged()
    {
        _cachedCachedUpgrade = UpgradeManager.Instance.Get(UpgradeType);
        RefreshStats();
    }

    protected virtual void InitializeUpgradeData()
    {
        UpgradeManager.OnDataChanged += OnUpgradeChanged;
        UpgradeManager.OnDataInitialized -= InitializeUpgradeData;
        OnUpgradeChanged();
    }
    
    protected virtual void Cleanup()
    {
        UpgradeManager.OnDataChanged -= OnUpgradeChanged;
    }
    
    protected abstract void RefreshStats();
}
