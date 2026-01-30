using System;
using System.Text.RegularExpressions;

public class Account
{
    public readonly string Email;
    public readonly string Password;
    
    private const string EmailPattern =
        @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
    private const string PasswordPattern =
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])[A-Za-z\d\W_]{7,20}$";
    
    public Account(string email, string password)
    {
        if (string.IsNullOrEmpty(email)) throw new System.ArgumentException("Email cannot be null or empty.");
        if (!Regex.IsMatch(email, EmailPattern)) throw new ArgumentException("Email must be a valid email address.");
        if (string.IsNullOrEmpty(password)) throw new ArgumentException("Password cannot be null or empty.");
        if (!Regex.IsMatch(password, PasswordPattern)) throw new ArgumentException("Password must be a valid password.");
        
        Email = email;
        Password = password;
    }
}

// 이메일 규칙
// 비어있으면 안된다.
// 올바른 이메일이어야 한다.
// 동일한 이메일으로 중복 가입이 불가능하다.
//
// 비밀번호 규칙
// 비어있으면 안된다.
// 6자리 이상 12자 이하여야 한다.
// 대문자를 1개 이상 포함해야 한다.
// 특수문자를 1개 이상 포함해야 한다.