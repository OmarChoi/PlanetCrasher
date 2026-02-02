using System.Text.RegularExpressions;

public class AccountEmailSpecification
{
    private const string EmailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

    private string _errorMessage;
    public string ErrorMessage => _errorMessage;
    
    public bool IsSatisfiedBy(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            _errorMessage = "Email cannot be null or empty";
            return false;
        }
        if (!Regex.IsMatch(email, EmailPattern))
        {
            _errorMessage = "Email is not a valid email address";
            return false;
        }
        return true;
    }
}