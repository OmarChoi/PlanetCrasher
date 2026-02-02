using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeItem : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    [SerializeField] private Image _icon;
    
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _description;
    
    [SerializeField] private TextMeshProUGUI _type;
    [SerializeField] private TextMeshProUGUI _count;

    [SerializeField] private Image _blockImage;
    
    private Upgrade _upgrade;

    private void Awake()
    {
        _button.onClick.AddListener(Upgrade);
    }
    
    public void Refresh(Upgrade upgrade)
    {
        if (upgrade == null) return;
        _upgrade = upgrade;
        _icon.sprite = upgrade.MetaData.Icon;
        _name.text = upgrade.MetaData.Name;
        _price.text = upgrade.Cost.ToString();
        _description.text = UpgradeManager.Instance.GetDescription(upgrade);
        _type.text = upgrade.MetaData.ClickType.ToString();
        _count.text = $"Lv.{upgrade.Level + 1}";
        
        bool canLevelUp = UpgradeManager.Instance.CanLevelUp(upgrade.MetaData.Type);
        _blockImage.gameObject.SetActive(!canLevelUp);
    }

    private void Upgrade()
    {
        if (_upgrade == null) return;

        if (!UpgradeManager.Instance.CanLevelUp(_upgrade.MetaData.Type)) return;
        UpgradeManager.Instance.TryLevelUp(_upgrade.MetaData.Type);
        // todo. 이펙트, 애니메이션, 트위닝
    }
}