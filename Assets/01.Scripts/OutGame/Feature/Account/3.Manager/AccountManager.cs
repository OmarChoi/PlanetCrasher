using System;
using UnityEngine;

// 1. 도메인 관리 : CRUD(생성/조회/수정/삭제)와 같은 비즈니스 로직
// 2. 외부와의 소통 창구
public class AccountManager : MonoBehaviour
{
    public AccountManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private bool TryLogin(string email, string password)
    {
        // todo. Repository에 있는 계정인지 확인 작업 진행
        try
        {
            Account account = new Account(email, password);
        }
        catch (Exception e)
        {
            return false;
        }

        if (!PlayerPrefs.HasKey(email)) return false;
        string passwordHash = PlayerPrefs.GetString(email);
        return passwordHash == password;
    }

    public bool TryRegister(string email, string password)
    {
        if (PlayerPrefs.HasKey(email)) return false;
        try
        {
            Account account = new Account(email, password);
        }
        catch (Exception e)
        {
            return false;
        }

        // todo. 새로운 계정 Repository에 등록(저장)
        string hashedPassword = PasswordHashService.ConvertPasswordToHash(password);
        Debug.Log(hashedPassword);
        PlayerPrefs.SetString(email, hashedPassword);
        PlayerPrefs.Save();

        return true;
    }

    public void Logout()
    {

    }
}