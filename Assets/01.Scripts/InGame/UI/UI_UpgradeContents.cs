using UnityEngine;
using System.Collections.Generic;

public class UI_UpgradeContents : MonoBehaviour
{
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _upgradeContentPrefab;
    
    [SerializeField] private List<UpgradeData> _upgradeDataList;

    private void Start()
    {
        InitializeUpgrades();
    }

    private void InitializeUpgrades()
    {
        foreach (var data in _upgradeDataList)
        {
            GameObject obj = Instantiate(_upgradeContentPrefab, _contentParent);
            UI_UpgradeContent content = obj.GetComponent<UI_UpgradeContent>();
            content.SetData(data);
        }
    }
}