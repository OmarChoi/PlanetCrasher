using System.Text.RegularExpressions;
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
    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordConfirmInputField;
    [SerializeField] private TextMeshProUGUI _messageTextUI;
    
    [Header("Message Colors")]
    [SerializeField] private Color _errorColor = Color.red;
    
    // Error Message
    private const string LoginErrorMessage = "잘못된 아이디 또는 비밀번호 입니다.";
    private string _errorMessage;

    private void Start()
    {
        AddButtonEvents();
        Refresh();
        _idInputField.text = AccountManager.Instance.GetLastLoginId();
        ClearMessage();
    }

    private void AddButtonEvents()
    {
        _gotoRegisterButton.onClick.AddListener(GotoRegister);
        _loginButton.onClick.AddListener(Login);
        _gotoLoginButton.onClick.AddListener(GotoLogin);
        _registerButton.onClick.AddListener(Register);
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

    private void Login()
    {
        string id = _idInputField.text;
        string password = _passwordInputField.text;
        AuthResult result = AccountManager.Instance.TryLogin(id, password);
        if (!result.Success)
        {
            ShowErrorMessage(result.ErrorMessage);
        }
    }

    private void Register()
    {
        string id = _idInputField.text;
        string password = _passwordInputField.text;
        string confirmPassword = _passwordConfirmInputField.text;
        if (string.IsNullOrEmpty(confirmPassword) || password != confirmPassword)
        {
            ShowErrorMessage("패스워드를 확인해주세요.");
            return;
        }

        AuthResult result = AccountManager.Instance.TryRegister(id, password);
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
        var emailSpec = new AccountEmailSpecification();
        if (!emailSpec.IsSatisfiedBy(email))
        {
            _loginButton.enabled = false;
            ShowErrorMessage(emailSpec.ErrorMessage);
            
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
