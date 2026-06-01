using Cysharp.Threading.Tasks;

// 로그인이 필요 없는 WebGL 빌드용 Mock 저장소.
// 입력과 무관하게 항상 게스트 계정으로 성공 처리한다.
public class MockAccountRepository : IAccountRepository
{
    public UniTask<AccountResult> Register(string email, string password)
    {
        return UniTask.FromResult
        (
            new AccountResult
            (
                success: true,
                errorMessage: string.Empty,
                account: new Account(GuestSession.Email)
            )
        );
    }

    public UniTask<AccountResult> Login(string email, string password)
    {
        return UniTask.FromResult
        (
            new AccountResult
            (
                success: true,
                errorMessage: string.Empty,
                account: new Account(GuestSession.Email)
            )
        );
    }

    public void Logout()
    {
        // 게스트 세션은 별도 정리 작업이 없다.
    }
}
