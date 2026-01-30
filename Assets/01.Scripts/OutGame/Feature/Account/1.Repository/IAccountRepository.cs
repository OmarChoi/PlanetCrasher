public interface IAccountRepository
{
    void SavePassword(string email, string hashedPassword);
    string LoadPassword(string email);
    bool Exists(string email);
}
