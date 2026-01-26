using UnityEngine;

public class UI_UpgradeContents : MonoBehaviour
{
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _upgradeContentPrefab;
    [SerializeField] private UpgradeContents _upgradeContents;

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        foreach (var upgradeData in _upgradeContents.UpgradeDataList)
        {
            GameObject obj = Instantiate(_upgradeContentPrefab, _contentParent);
            UI_UpgradeContent content = obj.GetComponent<UI_UpgradeContent>();

            UpgradeContentData contentData = _upgradeContents.GetContentData(upgradeData);
            content.SetData(contentData);
        }
    }
}