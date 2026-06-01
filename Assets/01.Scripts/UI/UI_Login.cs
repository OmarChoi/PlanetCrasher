using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Login : MonoBehaviour
{
    private enum ESceneMode
    {
        Login,
        Register
    }
    
    private ESceneMode _mode = ESceneMode.Login;
    
    [Header("Button")]
    [SerializeField] private GameObject _passwordConfirmObject;
    [SerializeField] private Button _gotoRegisterButton;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _gotoLoginButton;
    [SerializeField] private Button _registerButton;
    [SerializeField] private Button _guestButton;

    [Header("Text")]
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordConfirmInputField;
    [SerializeField] private TextMeshProUGUI _messageTextUI;
    
    [Header("Message Colors")]
    [SerializeField] private Color _errorColor = Color.red;

    private void Start()
    {
        AddButtonEvents();

        // 백엔드가 없는 빌드(WebGL)는 로그인/회원가입이 모두 단일 게스트 세이브로 귀결되므로,
        // 계정 의미론을 못 받쳐주는 거짓 UX 대신 인증 폼을 숨기고 '시작하기'(게스트) 진입만 노출한다.
        if (AppEnvironment.IsBackendless)
        {
            SetupGuestOnlyMode();
        }
        else
        {
            SetupGuestButton();
            Refresh();
            ClearMessage();
        }
    }

    private void AddButtonEvents()
    {
        _gotoRegisterButton.onClick.AddListener(GotoRegister);
        _loginButton.onClick.AddListener(OnLoginClicked);
        _gotoLoginButton.onClick.AddListener(GotoLogin);
        _registerButton.onClick.AddListener(OnRegisterClicked);
        _guestButton.onClick.AddListener(OnGuestClicked);
    }

    private void OnDestroy()
    {
        _gotoRegisterButton.onClick.RemoveListener(GotoRegister);
        _loginButton.onClick.RemoveListener(OnLoginClicked);
        _gotoLoginButton.onClick.RemoveListener(GotoLogin);
        _registerButton.onClick.RemoveListener(OnRegisterClicked);
        _guestButton.onClick.RemoveListener(OnGuestClicked);
    }

    private void SetupGuestButton()
    {
        if (_guestButton == null) return;
        _guestButton.gameObject.SetActive(true);
    }

    // 인증 폼(이메일/비밀번호/로그인/회원가입 전환)을 모두 숨기고 게스트 진입 버튼만 남긴다.
    private void SetupGuestOnlyMode()
    {
        _passwordConfirmObject.SetActive(false);
        _gotoRegisterButton.gameObject.SetActive(false);
        _loginButton.gameObject.SetActive(false);
        _gotoLoginButton.gameObject.SetActive(false);
        _registerButton.gameObject.SetActive(false);

        _emailInputField.gameObject.SetActive(false);
        _passwordInputField.gameObject.SetActive(false);
        _passwordConfirmInputField.gameObject.SetActive(false);

        if (_guestButton != null)
        {
            _guestButton.gameObject.SetActive(true);
        }

        ClearMessage();
    }

    private void OnLoginClicked()
    {
        Login().Forget();
    }

    private void OnRegisterClicked()
    {
        Register().Forget();
    }

    private void OnGuestClicked()
    {
        AccountManager.Instance.EnterAsGuest();
        SceneManager.LoadScene("GameScene");
    }

    private void Refresh()
    {
        _passwordConfirmObject.SetActive(_mode == ESceneMode.Register);
        _gotoRegisterButton.gameObject.SetActive(_mode == ESceneMode.Login);
        _loginButton.gameObject.SetActive(_mode == ESceneMode.Login);
        _gotoLoginButton.gameObject.SetActive(_mode == ESceneMode.Register);
        _registerButton.gameObject.SetActive(_mode == ESceneMode.Register);
        ClearMessage();
    }

    private async UniTaskVoid Login()
    {
        string email = _emailInputField.text;
        string password = _passwordInputField.text;
        AccountResult result = await AccountManager.Instance.TryLogin(email, password);
        if (result.Success)
        {
            SceneManager.LoadScene("GameScene"); 
        }
        else
        {
            ShowErrorMessage(result.ErrorMessage);
        }
    }

    private async UniTaskVoid Register()
    {
        string email = _emailInputField.text;
        string password = _passwordInputField.text;
        string confirmPassword = _passwordConfirmInputField.text;
        if (string.IsNullOrEmpty(confirmPassword) || password != confirmPassword)
        {
            ShowErrorMessage("Please check your password.");
            return;
        }

        AccountResult result = await AccountManager.Instance.TryRegister(email, password);
        if (result.Success)
        {
            GotoLogin();
        }
        else
        {
            ShowErrorMessage(result.ErrorMessage);
        }
    }
    
    private void ShowErrorMessage(string message)
    {
        if (_messageTextUI == null) return;
        _messageTextUI.text = message;
        _messageTextUI.color = _errorColor;
    }

    public void OnEmailTextChanged(string email)
    {
        var result = AccountManager.Instance.ValidateEmail(email);
        if (!result.IsValid)
        {
            _loginButton.enabled = false;
            ShowErrorMessage(result.ErrorMessage);
        }
        else
        {
            _loginButton.enabled = true;
            ClearMessage();
        }
    }
    
    private void ClearMessage()
    {
        if (_messageTextUI != null)
        {
            _messageTextUI.text = "";
        }
    }
    
    private void GotoLogin()
    {
        _mode = ESceneMode.Login;
        Refresh();
    }

    private void GotoRegister()
    {
        _mode = ESceneMode.Register;
        Refresh();
    }
}
