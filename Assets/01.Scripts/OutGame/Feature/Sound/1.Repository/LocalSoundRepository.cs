using UnityEngine;

public class LocalSoundRepository : ISoundRepository
{
    private const string Key = "SoundData";

    public void Save(SoundSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Key, json);
        PlayerPrefs.Save();
    }

    public SoundSaveData Load()
    {
        string json = PlayerPrefs.GetString(Key, "");
        if (string.IsNullOrEmpty(json))
        {
            return new SoundSaveData { BgmVolume = 1f, SfxVolume = 1f };
        }
        return JsonUtility.FromJson<SoundSaveData>(json);
    }
}