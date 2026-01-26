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

    private UpgradeData _upgradeData;

    private void Awake()
    {
        _button.onClick.AddListener(Upgrade);
    }

    // 데이터 설정 메서드
    public void SetData(UpgradeData data)
    {
        _upgradeData = data;
        
        _icon.sprite = data.Icon;
        _name.text = data.UpgradeName;
        _price.text = data.Price.ToString();
        _reward.text = data.RewardAmount.ToString();
        _type.text = data.Type.ToString();
        _count.text = $"x{data.Count}";
    }

    private void Upgrade()
    {
        // 업그레이드 로직
        Debug.Log("Upgrade");
    }
}