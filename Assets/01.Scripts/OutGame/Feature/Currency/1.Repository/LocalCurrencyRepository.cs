using UnityEngine;

public class LocalCurrencyRepository : ICurrencyRepository
{
    public void Save(CurrencySaveData saveData)
    {
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            PlayerPrefs.SetString(type.ToString(), saveData.Currencies[(int)type].ToString("G17"));
        }
    }

    public CurrencySaveData Load()
    {
        CurrencySaveData saveData = new CurrencySaveData();
        
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            if (PlayerPrefs.HasKey(i.ToString()))
            {
                saveData.Currencies[i] = double.Parse(PlayerPrefs.GetString(i.ToString(), "0"));
            }
        }
        return saveData;
    }
}
