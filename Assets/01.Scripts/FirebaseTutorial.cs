using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;

public class FirebaseTutorial : MonoBehaviour
{
    private FirebaseApp _app = null;
    private FirebaseAuth _auth = null;
    private FirebaseFirestore _db = null;

    [SerializeField] private TextMeshProUGUI _progressText;
    
    private void Start()
    {
        Init().Forget();
    }

    private async UniTaskVoid Init()
    {
        // 이 씬이 시작되면
        // 1. Firebase 초기화
        // 2. Logout
        // 3. 재로그인
        // 4. 강아지 추가
        _progressText.text = "파이어베이스 초기화 준비 중";
        Debug.Log("파이어베이스 초기화 대기");
        
        await InitFirebase();
        _progressText.text = "파이어베이스 초기화 완료";
        Debug.Log("파이어베이스 초기화 완료");
        
        Logout();
        _progressText.text = "로그아웃 완료";
        Debug.Log("로그아웃 완료");
        
        
        await LoginAsync("omarchoi80@skkukdp.re.kr", "12345678");
        _progressText.text = "로그인 완료";
        Debug.Log("로그인 완료");
        
        
        await SaveDogsAsync();
        _progressText.text = "강아지 저장 완료";
        Debug.Log("강아지 저장 완료");
    }

    private async UniTask InitFirebase()
    {
        try
        {
            DependencyStatus result = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
            if (result == DependencyStatus.Available)
            {
                _app = FirebaseApp.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;
                _db = FirebaseFirestore.DefaultInstance;
                Debug.Log("[FirebaseTutorial.cs] Firebase is available.");
            }
            else
            {
                Debug.LogError("[FirebaseTutorial.cs] Failed to initialize all Firebase objects.");
            }
        }
        catch (FirebaseException fe)
        {
            Debug.LogError("[FirebaseTutorial.cs] Failed to initialize with firebase error : " + fe.Message);
        }
        catch (Exception e)
        {
            Debug.LogError("[FirebaseTutorial.cs] Failed to initialize with unknown error : " + e.Message);
            throw;
        }
    }

    private void Update()
    {
        if (_app == null) return;
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Register("omarchoi80@skkukdp.re.kr", "12345678").Forget();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoginAsync("omarchoi80@skkukdp.re.kr", "12345678").Forget();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Logout();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CheckLoginStatus();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SaveDogsAsync().Forget();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            LoadMyDog();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            LoadDogs();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            DeleteDog();
        }
    }
    
    private void CheckLoginStatus()
    {
        FirebaseUser user = _auth.CurrentUser;
        if (user == null)
        {
            Debug.Log("Login available");
        }
        else
        {            
            Debug.LogFormat("Logging in : {0} ({1})", user.Email, user.UserId);
        }
    }
    
    private async UniTask Register(string email, string password)
    {
        try
        {
            var result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
        }
        catch (FirebaseException fe)
        {
            Debug.LogError("[FirebaseTutorial.cs] Failed to create user with firebase error: " + fe.Message);
        }
        catch (Exception e)
        {
            Debug.LogError("[FirebaseTutorial.cs] Failed to create user with unknown error: " + e.Message);
        }
    }

    private async UniTask LoginAsync(string email, string password)
    {
        try
        {
            var result = await _auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.Email, result.User.UserId);
        }
        catch (FirebaseException fe)
        {
            Debug.LogError("[FirebaseTutorial.cs] Failed to sign in with firebase error : " + fe.Message);
        }
        catch (Exception e)
        {
            Debug.LogError("[FirebaseTutorial.cs] Failed to sign in with unknown error : " + e.Message);
        }
    }

    private void Logout()
    {
        _auth.SignOut();
        Debug.Log("Logged out");
    }

    private async UniTask SaveDogsAsync()
    {
        DogSaveData dogSaveData = new DogSaveData
        (
            name: "댕댕이",
            age: 4
        );

        try
        {
            await _db.Collection("Dogs").Document("Omar_Dog").SetAsync(dogSaveData).AsUniTask();
            Debug.Log("Saved Dog Completely");
        }
        catch (FirebaseException fe)
        {
            Debug.LogError("[FirebaseTutorial.cs] Failed to save dog with firebase error : " + fe.Message);
        }
        catch (Exception e)
        {
            Debug.LogError("[FirebaseTutorial.cs] Failed to save dog with unknown error : " + e.Message);
        }
    }

    private void LoadMyDog()
    {
        _db.Collection("Dogs").Document("Omar_Dog").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                var snapshot = task.Result;
                DogSaveData myDog = snapshot.ConvertTo<DogSaveData>();
                Debug.Log($"Loading Dog Completely {myDog.Name} {myDog.Age}");
            }
            else
            {
                Debug.LogError("Failed to LoadDogs" + task.Exception);
            }
        });
    }

    private void LoadDogs()
    {
        _db.Collection("Dogs").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                QuerySnapshot snapshot = task.Result;
                Debug.Log("----------Dogs----------");
                foreach (DocumentSnapshot dog in snapshot.Documents)
                {
                    DogSaveData myDog = dog.ConvertTo<DogSaveData>();
                    Debug.Log($"{myDog.Name} {myDog.Age}");
                }
            }
            else
            {
                Debug.LogError("Failed to LoadDogs" + task.Exception);
            }
        });
    }

    private void DeleteDog()
    {
        _db.Collection("Dogs").WhereEqualTo("Name", "댕댕이").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                QuerySnapshot snapshot = task.Result;
                foreach (DocumentSnapshot dog in snapshot.Documents)
                {
                    DogSaveData myDog = dog.ConvertTo<DogSaveData>();
                    if (myDog.Name == "댕댕이")
                    {
                        // 삭제
                        _db.Collection("Dogs").Document(myDog.Id).DeleteAsync().ContinueWithOnMainThread(task =>
                        {
                            if (task.IsCompletedSuccessfully)
                            {
                                Debug.Log("데이터가 삭제됐습니다.");
                            }
                        });
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to LoadDogs" + task.Exception);
            }
        });
    }
}
