using UnityEngine;

public class LocalAccountRepository : IAccountRepository
{
    public void SavePassword(string email, string hashedPassword)
    {
        PlayerPrefs.SetString(email, hashedPassword);
        PlayerPrefs.Save();
    }

    public string LoadPassword(string email)
    {
        return PlayerPrefs.GetString(email, null);
    }

    public bool Exists(string email)
    {
        return PlayerPrefs.HasKey(email);
    }
}