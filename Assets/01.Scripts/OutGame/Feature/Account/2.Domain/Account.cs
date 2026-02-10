using System;

public class Account
{
    public readonly string Email;

    public Account(string email)
    {
        var emailSpec = new EmailSpecification();
        var emailResult = emailSpec.IsSatisfiedBy(email);
        if (!emailResult.IsValid) throw new ArgumentException(emailResult.ErrorMessage);

        Email = email;
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