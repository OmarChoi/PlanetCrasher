using System.Collections.Generic;
using UnityEngine;

public class UpgradeContents : MonoBehaviour
{
    [SerializeField] private List<UpgradeData> _upgradeDataList;

    private Dictionary<UpgradeData, UpgradeContentData> _contentDatas = new Dictionary<UpgradeData, UpgradeContentData>();

    public IReadOnlyDictionary<UpgradeData, UpgradeContentData> ContentDatas => _contentDatas;
    public List<UpgradeData> UpgradeDataList => _upgradeDataList;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _contentDatas.Clear();

        foreach (var upgradeData in _upgradeDataList)
        {
            _contentDatas[upgradeData] = new UpgradeContentData(upgradeData);
        }
    }

    public UpgradeContentData GetContentData(UpgradeData baseData)
    {
        return _contentDatas.GetValueOrDefault(baseData);
    }

    public List<UpgradeContentData> GetAllContentDatas()
    {
        return new List<UpgradeContentData>(_contentDatas.Values);
    }
}
