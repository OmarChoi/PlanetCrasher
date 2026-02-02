using System.Text.RegularExpressions;

public class PasswordSpecification : ISpecification<string>
{
    private const int MinLength = 7;
    private const int MaxLength = 20;

    public ValidationResult IsSatisfiedBy(string password)
    {
        if (string.IsNullOrEmpty(password)) return ValidationResult.Failure("Please enter a password.");
        if (password.Length < MinLength) return ValidationResult.Failure($"Password must be at least {MinLength} characters.");
        if (password.Length > MaxLength) return ValidationResult.Failure($"Password must be at most {MaxLength} characters.");
        if (!Regex.IsMatch(password, @"[a-z]")) return ValidationResult.Failure("Password must contain a lowercase letter.");
        if (!Regex.IsMatch(password, @"[A-Z]")) return ValidationResult.Failure("Password must contain an uppercase letter.");
        if (!Regex.IsMatch(password, @"[\W_]")) return ValidationResult.Failure("Password must contain a special character.");
        return ValidationResult.Success();
    }
}
