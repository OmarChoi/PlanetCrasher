using System;
using System.Text.RegularExpressions;

public class Account
{
    public readonly string Email;
    public readonly string HashedPassword;

    
    public Account(string email, string hashedPassword)
    {
        var emailSpec = new AccountEmailSpecification();
        if (!emailSpec.IsSatisfiedBy(email)) throw new ArgumentException(emailSpec.ErrorMessage);
        
        Email = email;
        HashedPassword = hashedPassword;
    }
}

// 이메일 규칙
// 비어있으면 안된다.
// 올바른 이메일이어야 한다.
// 동일한 이메일으로 중복 가입이 불가능하다.
//
// 비밀번호 규칙
// 비어있으면 안된다.
// 7자리 이상 20자 이하여야 한다.
// 대문자를 1개 이상 포함해야 한다.
// 특수문자를 1개 이상 포함해야 한다.