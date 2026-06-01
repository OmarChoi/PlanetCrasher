using Cysharp.Threading.Tasks;
using UnityEngine;

// 1. 도메인 관리 : CRUD(생성/조회/수정/삭제)와 같은 비즈니스 로직
// 2. 외부와의 소통 창구
public class AccountManager : Singleton<AccountManager>
{
    protected override bool IsPersistent => true;

    private Account _currentAccount = null;
    public bool IsLogin => _currentAccount != null;
    public string Email => _currentAccount?.Email;

    private IAccountRepository _accountRepository;

    private PasswordSpecification _passwordSpecification;
    private EmailSpecification _emailSpecification;

    protected override void Initialize()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        _accountRepository = new FirebaseAccountRepository();
#else
        _accountRepository = new MockAccountRepository();
#endif
        _passwordSpecification = new PasswordSpecification();
        _emailSpecification = new EmailSpecification();
    }

    public ValidationResult ValidateEmail(string email)
    {
        return _emailSpecification.IsSatisfiedBy(email);
    }

    // 로그인 없이 게스트로 진입한다(WebGL 빌드 전용 흐름).
    public void EnterAsGuest()
    {
        _currentAccount = new Account(GuestSession.Email);
    }

    public async UniTask<AccountResult> TryLogin(string email, string password)
    {
        var passwordResult = _passwordSpecification.IsSatisfiedBy(password);
        if (!passwordResult.IsValid)
        {
            return new AccountResult
            (
                success: false,
                errorMessage: passwordResult.ErrorMessage,
                account: null
            );
        }

        AccountResult result = await _accountRepository.Login(email, password);
        if (result.Success)
        {
            _currentAccount = result.Account;
        }
        return result;
    }

    public async UniTask<AccountResult> TryRegister(string email, string password)
    {
        var passwordResult = _passwordSpecification.IsSatisfiedBy(password);
        if (!passwordResult.IsValid)
        {
            return new AccountResult
            (
                success: false,
                errorMessage: passwordResult.ErrorMessage,
                account: null
            );
        }
        AccountResult result = await _accountRepository.Register(email, password);
        return result;
    }
    
    public void Logout()
    {
        _accountRepository.Logout();
        _currentAccount = null;
    }
}