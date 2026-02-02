using UnityEngine;

public class LocalAccountRepository : IAccountRepository
{
    public AuthResult Register(string email, string password)
    {
        if (IsExist(email))
        {
            return new AuthResult
            (
                success: false,
                errorMessage: "This account already exists.",
                account: null
            );
        }
        
        string hashedPassword = Crypto.ConvertPasswordToHash(password);
        PlayerPrefs.SetString(email, hashedPassword);

        return new AuthResult
        (
            success: true,
            errorMessage: string.Empty,
            account: new Account(email, hashedPassword)
        );
    }
    
    public AuthResult Login(string email, string password)
    {
        if (!IsExist(email))
        {
            return new AuthResult
            (
                success: false,
                errorMessage: "Please check your ID or password.",
                account: null
            );
        }
        
        string storedPassword = PlayerPrefs.GetString(email);
        if (Crypto.VerifyPassword(password, storedPassword))
        {
            return new AuthResult
            (
                success: true,
                errorMessage: string.Empty,
                account: new Account(email, storedPassword)
            );
        }
        else
        {
            return new AuthResult
            (
                success: false,
                errorMessage: "Please check your ID or password.",
                account: null
            );
        }
    }
    
    public void Logout()
    {
        // todo. 데이터 저장 및 로그아웃 관련 코드 작성
    }

    public bool IsExist(string email)
    {
        return PlayerPrefs.HasKey(email);
    }
}