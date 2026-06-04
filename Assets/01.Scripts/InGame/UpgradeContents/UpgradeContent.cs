using UnityEngine;

public abstract class UpgradeContent : MonoBehaviour
{
    protected abstract EUpgradeType UpgradeType { get; }

    private Upgrade _cachedUpgrade;
    protected Upgrade CachedUpgrade => _cachedUpgrade;

    // Level 0 = 미보유. 콘텐츠는 보유 상태에서만 동작한다.
    protected bool IsOwned => _cachedUpgrade != null && _cachedUpgrade.IsOwned;

    protected double GetEffectValue(EUpgradeEffectType type) => _cachedUpgrade.GetEffectValue(type);

    // 스포너가 Instantiate 직후(첫 Start/Update 이전) 호출해 씬 종속 참조(행성 등)를 주입한다.
    // 씬에 사전 배치되지 않는 콘텐츠를 위한 훅이므로 기본 구현은 비어 있다.
    public virtual void Bind(Planet planet) { }

    private void Awake()
    {
        Init();
        UpgradeManager.OnDataInitialized += InitializeUpgradeData;
        if (UpgradeManager.Instance?.IsInitialized == true)
        {
            InitializeUpgradeData();
        }
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
