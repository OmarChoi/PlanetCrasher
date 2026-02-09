#if !UNITY_WEBGL || UNITY_EDITOR
using System;
using Firebase;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class FirebaseInitializer : Singleton<FirebaseInitializer>
{
    protected override bool IsPersistent => true;

    private void Start()
    {
        InitFirebase().Forget();
    }

    private async UniTask InitFirebase()
    {
        try
        {
            DependencyStatus result = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
            if (result == DependencyStatus.Available)
            {
                Debug.Log("[FirebaseInitializer.cs] Firebase is available.");
            }
            else
            {
                Debug.LogError("[FirebaseInitializer.cs] Failed to initialize all Firebase objects.");
            }
        }
        catch (FirebaseException fe)
        {
            Debug.LogError("[FirebaseInitializer.cs] Failed to initialize with firebase error : " + fe.Message);
        }
        catch (Exception e)
        {
            Debug.LogError("[FirebaseInitializer.cs] Failed to initialize with unknown error : " + e.Message);
        }
    }
}
#endif
