using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeContent : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    [SerializeField] private Image _icon;
    
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _description;
    
    [SerializeField] private TextMeshProUGUI _type;
    [SerializeField] private TextMeshProUGUI _count;

    private UpgradeContentData _contentData;

    private void Awake()
    {
        _button.onClick.AddListener(Upgrade);
        _button.interactable = true;
    }

    private void OnEnable()
    {
        GameManager.OnUpgradeCompleted += UpdateUI;
    }

    private void OnDisable()
    {
        GameManager.OnUpgradeCompleted -= UpdateUI;
    }

    public void SetData(UpgradeContentData contentData)
    {
        _contentData = contentData;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_contentData == null) return;

        var baseData = _contentData.BaseData;

        _icon.sprite = baseData.Icon;
        _name.text = baseData.UpgradeName;
        _price.text = _contentData.CurrentPrice.ToFormattedString();
        _description.text = _contentData.GetDescription();
        _type.text = baseData.ClickType.ToString();
        _count.text = $"Lv.{_contentData.CurrentLevel + 1}";
    }

    private void Upgrade()
    {
        GameManager.Instance.TryUpgrade(_contentData);
    }
}