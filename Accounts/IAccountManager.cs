using Npgsql;

public interface IAccountManager
{
    Task Create(NpgsqlConnection connection, User user);
    Task GetUserGuid(NpgsqlConnection connection, string username);
    Task<bool> CheckUsernameRegistered(NpgsqlConnection connection, string username);
}
