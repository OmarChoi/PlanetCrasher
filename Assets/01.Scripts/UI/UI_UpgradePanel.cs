using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UI_UpgradePanel : MonoBehaviour
{
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _upgradeItemPrefab;
    [SerializeField] private EffectDescriptionTableSO _descriptionTable;
    
    private readonly List<UI_UpgradeItem> _upgradeItems = new List<UI_UpgradeItem>();
    private readonly StringBuilder _stringBuilder = new StringBuilder();

    private void Awake()
    {
        UpgradeManager.OnDataInitialized += Init;
    }

    private void OnDestroy()
    {
        UpgradeManager.OnDataInitialized -= Init;
        UpgradeManager.OnDataChanged -= Refresh;
        CurrencyManager.OnDataChanged -= Refresh;
    }

    private void Init()
    {
        List<Upgrade> upgrades = UpgradeManager.Instance.GetAll();
        foreach (Upgrade upgrade in upgrades)
        {
            GameObject obj = Instantiate(_upgradeItemPrefab, _contentParent);
            UI_UpgradeItem item = obj.GetComponent<UI_UpgradeItem>();
            _upgradeItems.Add(item);
            item.Refresh(upgrade, BuildDescription(upgrade));
        }
        UpgradeManager.OnDataInitialized -= Init;
        
        UpgradeManager.OnDataChanged += Refresh;
        CurrencyManager.OnDataChanged += Refresh;
    }
    
    private void Refresh()
    {
        List<Upgrade> upgrades = UpgradeManager.Instance.GetAll();
        for (var i = 0; i < _upgradeItems.Count; ++i)
        {
            _upgradeItems[i].Refresh(upgrades[i], BuildDescription(upgrades[i]));
        }
    }

    private string BuildDescription(Upgrade upgrade)
    {
        _stringBuilder.Clear();
        for (int i = 0; i < upgrade.MetaData.Effects.Length; i++)
        {
            if (i > 0) _stringBuilder.Append('\n');
            UpgradeEffect effect = upgrade.MetaData.Effects[i];
            double value = upgrade.GetEffectValue(effect.Type);
            string format = _descriptionTable.GetFormat(effect.Type);
            _stringBuilder.Append(string.Format(format, value.ToCompactString()));
        }
        return _stringBuilder.ToString();
    }
}