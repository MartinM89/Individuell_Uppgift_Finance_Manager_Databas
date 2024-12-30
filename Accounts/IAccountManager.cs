using Npgsql;

public interface IAccountManager
{
    void CreateUser(User user);
    void SetLoggedInUserId(string username);
    bool CheckIfUsernameRegistered(string username);
    Guid GetUserGuid(string username);
    bool GetLoggedInUsername(string username);
    void CreateUserBonusReward(User user);
}
