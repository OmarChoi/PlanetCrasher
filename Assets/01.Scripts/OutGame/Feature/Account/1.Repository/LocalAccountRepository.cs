using UnityEngine;

public class LocalAccountRepository : IAccountRepository
{
    public AuthResult Register(string email, string password)
    {
        if (!IsExist(email))
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "중복된 계정입니다."
            };
        }
        
        string hashedPassword = Crypto.ConvertPasswordToHash(password);
        PlayerPrefs.SetString(email, hashedPassword);

        return new AuthResult()
        {
            Success = true,
            Account = new Account(email, hashedPassword)
        };
    }
    
    public AuthResult Login(string email, string password)
    {
        if (!IsExist(email))
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "아이디와 비밀번호를 확인해주세요."
            };
        }
        
        string myPassword = PlayerPrefs.GetString(email);
        if (Crypto.VerifyPassword(password, myPassword))
        {
            return new AuthResult()
            {
                Success = true,
                Account = new Account(email, myPassword)
            };
        }
        else
        {
            return new AuthResult()
            {
                Success = false,
                ErrorMessage = "아이디와 비밀번호를 확인해주세요."
            };
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