using UnityEngine;

public abstract class UpgradeContent : MonoBehaviour
{
    protected abstract EUpgradeType UpgradeType { get; }

    private Upgrade _cachedUpgrade;
    protected Upgrade CachedUpgrade => _cachedUpgrade;

    protected double GetEffectValue(EUpgradeEffectType type) => _cachedUpgrade.GetEffectValue(type);

    private void Awake()
    {
        UpgradeManager.OnDataInitialized += InitializeUpgradeData;
        Init();
    }
    
    private void OnDestroy()
    {
        Cleanup();
    }

    protected virtual void Init() { }
    
    private void OnUpgradeChanged()
    {
        _cachedUpgrade = UpgradeManager.Instance.Get(UpgradeType);
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
