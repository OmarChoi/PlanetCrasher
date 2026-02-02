using UnityEngine;

public class LocalSoundRepository : ISoundRepository
{
    private const string Key = "SoundData";
    private readonly string _userId;

    public LocalSoundRepository(string userId)
    {
        _userId = userId;
    }
    
    public void Save(SoundSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString($"{_userId}_{Key}", json);
        PlayerPrefs.Save();
    }

    public SoundSaveData Load()
    {
        string json = PlayerPrefs.GetString($"{_userId}_{Key}", "");
        if (string.IsNullOrEmpty(json))
        {
            return new SoundSaveData { BgmVolume = 1f, SfxVolume = 1f };
        }
        return JsonUtility.FromJson<SoundSaveData>(json);
    }
}