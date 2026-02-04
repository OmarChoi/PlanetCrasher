using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;

public class FirebaseAccountRepository : IAccountRepository
{
    private readonly FirebaseAuth _auth;
    
    public FirebaseAccountRepository()
    {
        _auth = FirebaseAuth.DefaultInstance;
    }
    
    public async UniTask<AccountResult> Register(string email, string password)
    {
        try
        {
            var result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();
        }
        catch (FirebaseException fe)
        {
            return new AccountResult
            (
                success: false,
                errorMessage: "Failed to create user with " + fe.ErrorCode,
                account: null
            );
        }
        catch (Exception e)
        {
            return new AccountResult
            (
                success: false,
                errorMessage: "Failed to create user with " + e.Message,
                account: null
            );
        }

        return new AccountResult
        (
            success: true,
            errorMessage: string.Empty,
            account: new Account(email, password)
        );
    }
    
    public async UniTask<AccountResult> Login(string email, string password)
    {
        string errorMessage = string.Empty;
        try
        {
            await _auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
            return new AccountResult
            (
                success: true,
                errorMessage: errorMessage,
                account: new Account(email, password)
            );
        }
        catch (FirebaseException fe)
        {
            errorMessage = "Failed to sign in user " + fe.ErrorCode;
        }
        catch (Exception e)
        {
            errorMessage = "Failed to sign in user " + e.Message;
        }
        
        return new AccountResult
        (
            success: true,
            errorMessage: errorMessage,
            account: new Account(email, password)
        );
    }
    
    
    public void Logout()
    {
        _auth.SignOut();
    }
}
