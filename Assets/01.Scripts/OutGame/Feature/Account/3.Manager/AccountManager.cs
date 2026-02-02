using UnityEngine;
using UnityEngine.SceneManagement;

// 1. 도메인 관리 : CRUD(생성/조회/수정/삭제)와 같은 비즈니스 로직
// 2. 외부와의 소통 창구
public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; private set; }
    private Account _currentAccount = null;
    public bool IsLogin => _currentAccount != null;
    public string Email => _currentAccount?.Email;

    private IAccountRepository _accountRepository;

    private PasswordSpecification _passwordSpecification;
    private EmailSpecification _emailSpecification;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _accountRepository = new LocalAccountRepository();
        _passwordSpecification = new PasswordSpecification();
        _emailSpecification = new EmailSpecification();
    }

    public ValidationResult ValidateEmail(string email)
    {
        return _emailSpecification.IsSatisfiedBy(email);
    }

    public AuthResult TryLogin(string email, string password)
    {
        var passwordResult = _passwordSpecification.IsSatisfiedBy(password);
        if (!passwordResult.IsValid)
        {
            return new AuthResult
            (
                success: false,
                errorMessage: passwordResult.ErrorMessage,
                account: null
            );
        }

        AuthResult result = _accountRepository.Login(email, password);
        if (result.Success)
        {
            _currentAccount = result.Account;
        }
        return result;
    }

    public AuthResult TryRegister(string email, string password)
    {
        var passwordResult = _passwordSpecification.IsSatisfiedBy(password);
        if (!passwordResult.IsValid)
        {
            return new AuthResult
            (
                success: false,
                errorMessage: passwordResult.ErrorMessage,
                account: null
            );
        }
        AuthResult result = _accountRepository.Register(email, password);
        return result;
    }
    
    public void Logout()
    {
        _accountRepository.Logout();
    }
}