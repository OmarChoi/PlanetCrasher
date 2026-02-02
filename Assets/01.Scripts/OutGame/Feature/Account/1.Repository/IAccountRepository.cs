public interface IAccountRepository
{
    AuthResult Register(string email, string hashedPassword);
    AuthResult Login(string email, string hashedPassword);
    void Logout();
    bool IsExist(string email);
}
