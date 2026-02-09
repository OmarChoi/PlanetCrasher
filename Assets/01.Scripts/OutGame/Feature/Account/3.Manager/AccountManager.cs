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
        _accountRepository = new FirebaseAccountRepository();
        _passwordSpecification = new PasswordSpecification();
        _emailSpecification = new EmailSpecification();
    }

    public ValidationResult ValidateEmail(string email)
    {
        return _emailSpecification.IsSatisfiedBy(email);
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
    }
}