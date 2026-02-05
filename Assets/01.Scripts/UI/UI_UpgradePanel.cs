using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_UpgradePanel : MonoBehaviour
{
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _upgradeItemPrefab;
    private readonly List<UI_UpgradeItem> _upgradeItems = new List<UI_UpgradeItem>();

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
        foreach (Upgrade upgradeData in upgrades)
        {
            GameObject obj = Instantiate(_upgradeItemPrefab, _contentParent);
            UI_UpgradeItem item = obj.GetComponent<UI_UpgradeItem>();
            _upgradeItems.Add(item);
            item.Refresh(upgradeData);
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
            _upgradeItems[i].Refresh(upgrades[i]);
        }
    }
}