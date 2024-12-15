using Npgsql;

public interface IAccountManager
{
    void Create(User user);
    void GetUserGuid(string username);
    bool CheckIfUsernameRegistered(string username);
}
