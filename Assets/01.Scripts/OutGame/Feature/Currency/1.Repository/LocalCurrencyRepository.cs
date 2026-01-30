using UnityEngine;

public class LocalCurrencyRepository : ICurrencyRepository
{
    public void Save(CurrencySaveData saveData)
    {
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            PlayerPrefs.SetString(type.ToString(), saveData.Currencies[(int)type].ToString());
        }
    }

    public CurrencySaveData Load()
    {
        CurrencySaveData saveData = CurrencySaveData.Default;
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            if (PlayerPrefs.HasKey(type.ToString()))
            {
                saveData.Currencies[i] = double.Parse(PlayerPrefs.GetString(type.ToString(), "0"));
            }
        }
        return saveData;
    }
}
