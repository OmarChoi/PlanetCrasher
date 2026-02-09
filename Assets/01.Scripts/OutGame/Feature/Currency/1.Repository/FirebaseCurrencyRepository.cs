#if !UNITY_WEBGL || UNITY_EDITOR
using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseCurrencyRepository : ICurrencyRepository
{
    private const string CURRENCY_COLLECTION_NAME = "Currency";
    
    private readonly FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
    private readonly FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;
    
    public async UniTaskVoid Save(CurrencySaveData saveData)
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            await _db.Collection(CURRENCY_COLLECTION_NAME).Document(email).SetAsync(saveData);
        }
        catch (FirebaseException fe)
        {
            Debug.LogError("[FirebaseCurrencyRepository.cs] Failed to save currency: " + fe.ErrorCode);
        }
        catch (Exception e)
        {
            Debug.LogError("[FirebaseCurrencyRepository.cs] Failed to save currency: " + e.Message);
        }
    }
    
    public async UniTask<CurrencySaveData> Load()
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            DocumentSnapshot snapshot = await _db.Collection(CURRENCY_COLLECTION_NAME).Document(email).GetSnapshotAsync();
            CurrencySaveData data = snapshot.ConvertTo<CurrencySaveData>();
            return data ?? CurrencySaveData.Default;
        }
        catch (FirebaseException fe)
        {
            Debug.LogError("[FirebaseCurrencyRepository.cs] Failed to load currency: " + fe.ErrorCode);
        }
        catch (Exception e)
        {
            Debug.LogError("[FirebaseCurrencyRepository.cs] Failed to load currency: " + e.Message);
        }
        return CurrencySaveData.Default;
    }
}
#endif