using UnityEngine;

public abstract class UpgradeContent : MonoBehaviour
{
    protected abstract EUpgradeType UpgradeType { get; }

    private Upgrade _cachedUpgrade;
    protected Upgrade CachedUpgrade => _cachedUpgrade;

    // Level 0 = 미보유. 콘텐츠는 보유 상태에서만 동작한다.
    protected bool IsOwned => _cachedUpgrade != null && _cachedUpgrade.IsOwned;

    // 자동공격 콘텐츠는 미보유 시 GameObject를 비활성화해 비주얼/동작을 함께 숨긴다.
    protected virtual bool DeactivateWhenUnowned => false;

    protected double GetEffectValue(EUpgradeEffectType type) => _cachedUpgrade.GetEffectValue(type);

    private void Awake()
    {
        UpgradeManager.OnDataInitialized += InitializeUpgradeData;
        if (UpgradeManager.Instance?.IsInitialized == true)
        {
            InitializeUpgradeData();
        }
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

        // 정적 이벤트 핸들러는 비활성 GameObject에서도 호출되므로,
        // 구매 시 OnDataChanged → 이 메서드가 다시 불려 SetActive(true)로 복귀할 수 있다.
        if (DeactivateWhenUnowned)
        {
            gameObject.SetActive(IsOwned);
        }
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
