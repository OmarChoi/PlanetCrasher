using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalAccountRepository : IAccountRepository
{
    public UniTask<AccountResult> Register(string email, string password)
    {
        // 비밀번호 검증
        var passwordSpec = new PasswordSpecification();
        var passwordResult = passwordSpec.IsSatisfiedBy(password);
        if (!passwordResult.IsValid)
        {
            return UniTask.FromResult
            (
                new AccountResult
                (
                    success: false,
                    errorMessage: passwordResult.ErrorMessage,
                    account: null
                )
            );
        }

        if (IsExist(email))
        {
            return UniTask.FromResult
            (
                new AccountResult
                (
                    success: false,
                    errorMessage: "This account already exists.",
                    account: null
                )
            );
        }

        string hashedPassword = Crypto.ConvertPasswordToHash(password);
        PlayerPrefs.SetString(email, hashedPassword);

        return UniTask.FromResult
        (
            new AccountResult
            (
                success: true,
                errorMessage: string.Empty,
                account: new Account(email)
            )
        );
    }

    public UniTask<AccountResult> Login(string email, string password)
    {
        if (!IsExist(email))
        {
            return UniTask.FromResult
            (
                new AccountResult
                (
                    success: false,
                    errorMessage: "Please check your ID or password.",
                    account: null
                )
            );
        }

        string storedPassword = PlayerPrefs.GetString(email);
        if (Crypto.VerifyPassword(password, storedPassword))
        {
            return UniTask.FromResult
            (
                new AccountResult
                (
                    success: true,
                    errorMessage: string.Empty,
                    account: new Account(email)
                )
            );
        }
        else
        {
            return UniTask.FromResult
            (
                new AccountResult
                (
                    success: false,
                    errorMessage: "Please check your ID or password.",
                    account: null
                )
            );
        }
    }

    public void Logout()
    {
        // todo. 데이터 저장 및 로그아웃 관련 코드 작성
    }

    private bool IsExist(string email)
    {
        return PlayerPrefs.HasKey(email);
    }
}