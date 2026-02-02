using System.Text.RegularExpressions;

public class AccountPasswordSpecification
{
    private const string PasswordPattern =
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])[A-Za-z\d\W_]{7,20}$";
    
    
    private string _errorMessage;
    public string ErrorMessage => _errorMessage;
    
    public bool IsSatisfiedBy(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            _errorMessage = "Password cannot be null or empty.";
            return false;
        }
        
        if (!Regex.IsMatch(password, PasswordPattern))
        {
            _errorMessage = "Password must be a valid password.";
            return false;
        }
        return true;
    }
}