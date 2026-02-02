// 인증 결과
// 회원 가입에 대한 에러 메세지
public readonly struct AuthResult
{
    public readonly bool Success;
    public readonly string ErrorMessage;
    public readonly Account Account;
    
    public AuthResult(bool success, string errorMessage, Account account)
    {
        Success = success;
        ErrorMessage = errorMessage;
        Account = account;
    }
}