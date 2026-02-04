using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalUpgradeRepository : IUpgradeRepository
{
    private const string Key = "UpgradeSaveData";
    private readonly string _userId;
    
    public LocalUpgradeRepository(string userId)
    {
        _userId = userId;
    }
    
    public async UniTaskVoid Save(UpgradeSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString($"{_userId}_{Key}", json);
        PlayerPrefs.Save();
    }
    
    public async UniTask<UpgradeSaveData> Load()
    {
        string json = PlayerPrefs.GetString($"{_userId}_{Key}", "");
        if (string.IsNullOrEmpty(json)) return UpgradeSaveData.Default;
        return JsonUtility.FromJson<UpgradeSaveData>(json);
    }
}