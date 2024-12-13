using Npgsql;

public interface IAccountManager
{
    void Create(NpgsqlConnection connection, User user);
    void GetUserGuid(NpgsqlConnection connection, string username);
    bool CheckUsernameRegistered(NpgsqlConnection connection, string username);
}
