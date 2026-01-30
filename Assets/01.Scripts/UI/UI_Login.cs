using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
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

    // 정규 표현식
    private const string EmailPattern =
        @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
    private const string PasswordPattern =
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])[A-Za-z\d\W_]{7,20}$";
    
    private void Start()
    {
        AddButtonEvents();
        Refresh();
        _idInputField.text = LoginManager.Instance.GetLastLoginId();
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
        if (!IsValidID(id))
        {
            ShowErrorMessage(LoginErrorMessage);
            return;
        }
        
        string password = _passwordInputField.text;
        if (!IsValidPassword(password))
        {
            ShowErrorMessage(LoginErrorMessage);
            return;
        }

        bool success = LoginManager.Instance.TryLogin(id, password);
        if (!success)
        {
            ShowErrorMessage(LoginErrorMessage);
            return;
        }
        SceneManager.LoadSceneAsync("GameScene");
    }

    private void Register()
    {
        string id = _idInputField.text;

        if (!IsValidID(id))
        {
            ShowErrorMessage(_errorMessage);
            return;
        }

        string password = _passwordInputField.text;
        if (!IsValidPassword(password))
        {
            ShowErrorMessage(_errorMessage);
            return;
        }

        string confirmPassword = _passwordConfirmInputField.text;
        if (string.IsNullOrEmpty(confirmPassword) || password != confirmPassword)
        {
            ShowErrorMessage("패스워드를 확인해주세요.");
            return;
        }
        
        bool success = LoginManager.Instance.Register(id, password, out string message);
        
        if (!success)
        {
            ShowErrorMessage(message);
            return;
        }
        GotoLogin();
    }
    
    private bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            ShowErrorMessage("패스워드를 입력해주세요.");
            return false;
        }
        if (!Regex.IsMatch(password, PasswordPattern))
        {
            ShowErrorMessage("잘못된 비밀번호 형식입니다.");
            return false;
        }
        return true;
    }

    private bool IsValidID(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            _errorMessage = "아이디를 확인해주세요.";
            return false;
        }
        if (!Regex.IsMatch(id, EmailPattern))
        {
            _errorMessage = "잘못된 아이디 형식입니다.";
            return false;
        }
        return true;
    }
    
    private void ShowErrorMessage(string message)
    {
        if (_messageTextUI != null)
        {
            _messageTextUI.text = message;
            _messageTextUI.color = _errorColor;
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
