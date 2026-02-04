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
        _= Init();
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
        var result = await FirebaseApp.CheckAndFixDependenciesAsync();
        
        if (result == DependencyStatus.Available)
        {
            _app = FirebaseApp.DefaultInstance;
            _auth = FirebaseAuth.DefaultInstance;
            _db = FirebaseFirestore.DefaultInstance;
            Debug.Log("Firebase is available.");
        }
        else
        {
            Debug.LogError("Failed to initialize Firebase " + result);
        }
    }

    private void Update()
    {
        if (_app == null) return;
        _= ProcessInput();
    }

    private async UniTaskVoid ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Register("omarchoi80@skkukdp.re.kr", "12345678");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            await LoginAsync("omarchoi80@skkukdp.re.kr", "12345678");
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
            await SaveDogsAsync();
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
    
    private void Register(string email, string password)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) 
            {
                Debug.LogError("Canceled to Register");
                return;
            }
            if (task.IsFaulted) 
            {
                Debug.LogError("Failed to Register with Error : " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
        });
    }

    private async UniTask LoginAsync(string email, string password)
    {
        try
        {
            var result = await _auth.SignInWithEmailAndPasswordAsync(email, password);
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.Email, result.User.UserId);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to Login with Error : " + ex);
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
            await _db.Collection("Dogs").Document("Omar_Dog").SetAsync(dogSaveData);
            Debug.Log("Saved Dog Completely");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to SaveDogs: " + ex);
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
