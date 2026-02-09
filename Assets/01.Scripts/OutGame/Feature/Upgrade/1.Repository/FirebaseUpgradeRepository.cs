#if !UNITY_WEBGL || UNITY_EDITOR
using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseUpgradeRepository : IUpgradeRepository
{
    private const string UPGRADE_COLLECTION_NAME = "Upgrade";
    
    private readonly FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
    private readonly FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;
    
    public async UniTaskVoid Save(UpgradeSaveData data)
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            await _db.Collection(UPGRADE_COLLECTION_NAME).Document(email).SetAsync(data);
        }
        catch (FirebaseException fe)
        {
            Debug.LogError("[FirebaseUpgradeRepository.cs] Failed to save Upgrade: " + fe.ErrorCode);
        }
        catch (Exception e)
        {
            Debug.LogError("[FirebaseUpgradeRepository.cs] Failed to save Upgrade: " + e.Message);
        }
    }
    
    public async UniTask<UpgradeSaveData> Load()
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            DocumentSnapshot snapshot = await _db.Collection(UPGRADE_COLLECTION_NAME).Document(email).GetSnapshotAsync();
            UpgradeSaveData data = snapshot.ConvertTo<UpgradeSaveData>();
            return data ?? UpgradeSaveData.Default;
        }
        catch (FirebaseException fe)
        {
            Debug.LogError("[FirebaseUpgradeRepository.cs] Failed to load currency: " + fe.ErrorCode);
        }
        catch (Exception e)
        {
            Debug.LogError("[FirebaseUpgradeRepository.cs] Failed to load currency: " + e.Message);
        }
        return UpgradeSaveData.Default;
    }
}
#endif
