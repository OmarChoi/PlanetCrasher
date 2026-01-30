using UnityEngine;

public class LocalUpgradeRepository : IUpgradeRepository
{
    private const string Key = "UpgradeSaveData";
    public void Save(UpgradeSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Key, json);
        PlayerPrefs.Save();
    }
    
    public UpgradeSaveData Load()
    {
        string json = PlayerPrefs.GetString(Key, "");
        if (string.IsNullOrEmpty(json)) return UpgradeSaveData.Default;
        return JsonUtility.FromJson<UpgradeSaveData>(json);
        
    }
}