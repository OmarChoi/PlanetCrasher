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

    private IAccountRepository _accountRepository;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _accountRepository = new LocalAccountRepository();
    }

    public AuthResult TryLogin(string email, string password)
    {
        if (!_accountRepository.Exists(email))
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "존재하지 않는 이메일 입니다."
            };
        }

        string hashedPassword = _accountRepository.LoadPassword(email);
        if (!PasswordHashService.VerifyPassword(password, hashedPassword))
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "잘못된 비밀번호 입니다."
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

        _currentAccount = account;
        PlayerPrefs.SetString(LastLoginEmail, email);
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("GameScene");

        return new AuthResult
        {
            Success = true,
            Account = account
        };
    }

    public AuthResult TryRegister(string email, string password)
    {
        if (_accountRepository.Exists(email))
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

        string hashedPassword = PasswordHashService.ConvertPasswordToHash(password);
        _accountRepository.SavePassword(email, hashedPassword);

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