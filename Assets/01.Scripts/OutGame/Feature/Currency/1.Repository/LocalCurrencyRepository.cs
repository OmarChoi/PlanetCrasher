using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalCurrencyRepository : ICurrencyRepository
{
    private readonly string _userId;

    public LocalCurrencyRepository(string userId)
    {
        _userId = userId;
    }
    
    public async UniTaskVoid Save(CurrencySaveData saveData)
    {
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            PlayerPrefs.SetString($"{_userId}_{type.ToString()}", saveData.Currencies[(int)type].ToString());
        }
    }

    public async UniTask<CurrencySaveData> Load()
    {
        CurrencySaveData saveData = CurrencySaveData.Default;
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            if (PlayerPrefs.HasKey($"{_userId}_{type.ToString()}"))
            {
                saveData.Currencies[i] = double.Parse(PlayerPrefs.GetString($"{_userId}_{type.ToString()}", "0"));
            }
        }
        return saveData;
    }
}
