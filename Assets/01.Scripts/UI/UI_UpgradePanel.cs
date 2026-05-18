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
    private string[] _cachedDescriptions;
    private int[] _cachedLevels;

    private void Awake()
    {
        UpgradeManager.OnDataInitialized += Init;
        if (UpgradeManager.Instance?.IsInitialized == true)
        {
            Init();
        }
    }

    private void OnDestroy()
    {
        UpgradeManager.OnDataInitialized -= Init;
        UpgradeManager.OnDataChanged -= Refresh;
        CurrencyManager.OnDataChanged -= Refresh;
    }

    private void Init()
    {
        IReadOnlyList<Upgrade> upgrades = UpgradeManager.Instance.GetAll();
        _cachedDescriptions = new string[upgrades.Count];
        _cachedLevels = new int[upgrades.Count];
        for (int i = 0; i < upgrades.Count; i++)
        {
            Upgrade upgrade = upgrades[i];
            GameObject obj = Instantiate(_upgradeItemPrefab, _contentParent);
            UI_UpgradeItem item = obj.GetComponent<UI_UpgradeItem>();
            _upgradeItems.Add(item);
            item.Refresh(upgrade, GetDescription(i, upgrade));
        }
        UpgradeManager.OnDataInitialized -= Init;

        UpgradeManager.OnDataChanged += Refresh;
        CurrencyManager.OnDataChanged += Refresh;
    }

    private void Refresh()
    {
        IReadOnlyList<Upgrade> upgrades = UpgradeManager.Instance.GetAll();
        for (var i = 0; i < _upgradeItems.Count; ++i)
        {
            _upgradeItems[i].Refresh(upgrades[i], GetDescription(i, upgrades[i]));
        }
    }

    private string GetDescription(int index, Upgrade upgrade)
    {
        if (_cachedDescriptions[index] != null && _cachedLevels[index] == upgrade.Level)
        {
            return _cachedDescriptions[index];
        }
        _cachedLevels[index] = upgrade.Level;
        _cachedDescriptions[index] = BuildDescription(upgrade);
        return _cachedDescriptions[index];
    }

    private string BuildDescription(Upgrade upgrade)
    {
        _stringBuilder.Clear();
        UpgradeEffect[] effects = upgrade.MetaData.Effects;
        for (int i = 0; i < effects.Length; i++)
        {
            if (i > 0) _stringBuilder.Append('\n');
            UpgradeEffect effect = effects[i];
            double value = upgrade.GetEffectValue(effect.Type);
            string format = _descriptionTable.GetFormat(effect.Type);
            _stringBuilder.AppendFormat(format, value.ToCompactString());
        }
        return _stringBuilder.ToString();
    }
}