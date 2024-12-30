using Npgsql;

public interface IAccountManager
{
    Task CreateUser(User user);
    Task SetLoggedInUserId(string username);
    Task<bool> CheckIfUsernameRegistered(string username);
    Task<Guid> GetUserGuid(string username);
    Task<bool> GetLoggedInUsername(string username);
    Task CreateUserBonusReward(User user);
}
