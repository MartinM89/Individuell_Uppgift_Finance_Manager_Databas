using Npgsql;

public interface IAccountManager
{
    void CreateUser(User user);
    void SetLoggedInUserId(string username);
    bool CheckIfUsernameRegistered(string username);
}
