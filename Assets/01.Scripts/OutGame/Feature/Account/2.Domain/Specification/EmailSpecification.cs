using System.Text.RegularExpressions;

public class EmailSpecification : ISpecification<string>
{
    private const string EmailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

    public ValidationResult IsSatisfiedBy(string email)
    {
        if (string.IsNullOrEmpty(email)) return ValidationResult.Failure("Please enter an email.");
        if (!Regex.IsMatch(email, EmailPattern)) return ValidationResult.Failure("Please enter a valid email address.");
        return ValidationResult.Success();
    }
}
