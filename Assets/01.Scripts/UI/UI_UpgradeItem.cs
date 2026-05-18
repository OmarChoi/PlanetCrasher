using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeItem : MonoBehaviour
{
    private static readonly string[] _clickTypeNames =
    {
        nameof(EClickType.PerClick),
        nameof(EClickType.AutoClick),
    };

    [SerializeField] private Button _button;

    [SerializeField] private Image _icon;

    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _description;

    [SerializeField] private TextMeshProUGUI _type;
    [SerializeField] private TextMeshProUGUI _count;

    [SerializeField] private Image _blockImage;

    private Upgrade _upgrade;
    private bool _staticInitialized;
    private int _cachedLevel = -1;
    private bool _cachedBlocked;
    private bool _cachedBlockedValid;

    private void Awake()
    {
        _button.onClick.AddListener(Upgrade);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(Upgrade);
    }

    public void Refresh(Upgrade upgrade, string description)
    {
        if (upgrade == null) return;
        _upgrade = upgrade;

        if (!_staticInitialized)
        {
            _icon.sprite = upgrade.MetaData.Icon;
            _name.text   = upgrade.MetaData.Name;
            _type.text   = _clickTypeNames[(int)upgrade.MetaData.ClickType];
            _staticInitialized = true;
        }

        if (_cachedLevel != upgrade.Level)
        {
            _cachedLevel = upgrade.Level;
            _price.text = upgrade.Cost.ToString();
            _count.text = $"Lv.{upgrade.Level + 1}";
            _cachedBlockedValid = false;
        }

        _description.text = description;

        bool blocked = !UpgradeManager.Instance.CanLevelUp(upgrade.MetaData.Type);
        if (!_cachedBlockedValid || _cachedBlocked != blocked)
        {
            _cachedBlocked = blocked;
            _cachedBlockedValid = true;
            _blockImage.gameObject.SetActive(blocked);
        }
    }

    private void Upgrade()
    {
        if (_upgrade == null) return;
        UpgradeManager.Instance.TryLevelUp(_upgrade.MetaData.Type);
    }
}
