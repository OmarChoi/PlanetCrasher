// 인증 결과
// 회원 가입에 대한 에러 메세지
public struct AuthResult
{
    public bool Success;
    public string ErrorMessage;
    public Account Account;
    
    public AuthResult(bool success, string errorMessage, Account account)
    {
        Success = success;
        ErrorMessage = errorMessage;
        Account = account;
    }
}