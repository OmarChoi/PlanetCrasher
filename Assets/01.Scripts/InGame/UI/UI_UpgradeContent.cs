using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeContent : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    [SerializeField] private Image _icon;
    
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _reward;
    
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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnUpgradeCompleted += UpdateUI;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnUpgradeCompleted -= UpdateUI;
        }
    }

    public void SetData(UpgradeContentData contentData)
    {
        _contentData = contentData;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (_contentData == null) return;

        var baseData = _contentData.BaseData;

        _icon.sprite = baseData.Icon;
        _name.text = baseData.UpgradeName;
        _price.text = _contentData.CurrentPrice.ToString();
        _reward.text = _contentData.GetCurrentReward().ToString();
        _type.text = baseData.Type.ToString();
        _count.text = $"Lv.{_contentData.CurrentLevel + 1}";
    }

    private void Upgrade()
    {
        if (GameManager.Instance.TryUpgrade(_contentData))
        {
            UpdateUI();
        }
    }
}