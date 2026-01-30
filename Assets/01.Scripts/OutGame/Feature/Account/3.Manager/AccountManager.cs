using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// 1. 도메인 관리 : CRUD(생성/조회/수정/삭제)와 같은 비즈니스 로직
// 2. 외부와의 소통 창구
public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; private set; }
    private Account _currentAccount = null;
    public bool IsLogin => _currentAccount != null;
    public string Email => _currentAccount.Email;
    private const string LastLoginEmail = "LastLoginEmail";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public AuthResult TryLogin(string email, string password)
    {
        // todo. Repository에 있는 계정인지 확인 작업 진행
        Account account = null;
        try
        {
            account = new Account(email, password);
        }
        catch (Exception e)
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }

        if (!PlayerPrefs.HasKey(email))
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "존재하지 않는 이메일 입니다."
            };
        }
        string hashedPassword = PlayerPrefs.GetString(email);
        if (!PasswordHashService.VerifyPassword(password, hashedPassword))
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "잘못된 비밀번호 입니다."
            };
        }
        _currentAccount = account;
        SceneManager.LoadSceneAsync("GameScene");
        return new AuthResult
        {
            Success = true,
            Account = account
        };
    }

    public AuthResult TryRegister(string email, string password)
    {
        if (PlayerPrefs.HasKey(email))
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "중복된 이메일 입니다.",
            };
        }
        Account account = null;
        try
        {
            account = new Account(email, password);
        }
        catch (Exception e)
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }

        // todo. 새로운 계정 Repository에 등록(저장)
        string hashedPassword = PasswordHashService.ConvertPasswordToHash(password);
        PlayerPrefs.SetString(email, hashedPassword);
        PlayerPrefs.Save();

        return new AuthResult
        {
            Success = true,
            Account = account
        };
    }
    
    public string GetLastLoginId()
    {
        return PlayerPrefs.GetString(LastLoginEmail, "");
    }

    public void Logout()
    {

    }
}