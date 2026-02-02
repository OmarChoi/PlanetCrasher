public interface IAccountRepository
{
    AuthResult Register(string email, string password);
    AuthResult Login(string email, string password);
    void Logout();
    bool IsExist(string email);
}
