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
        Refresh();
        ClearMessage();
    }

    private void AddButtonEvents()
    {
        _gotoRegisterButton.onClick.AddListener(GotoRegister);
        _loginButton.onClick.AddListener(OnLoginClicked);
        _gotoLoginButton.onClick.AddListener(GotoLogin);
        _registerButton.onClick.AddListener(OnRegisterClicked);
    }

    private void OnDestroy()
    {
        _gotoRegisterButton.onClick.RemoveListener(GotoRegister);
        _loginButton.onClick.RemoveListener(OnLoginClicked);
        _gotoLoginButton.onClick.RemoveListener(GotoLogin);
        _registerButton.onClick.RemoveListener(OnRegisterClicked);
    }

    private void OnLoginClicked()
    {
        Login().Forget();
    }

    private void OnRegisterClicked()
    {
        Register().Forget();
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
