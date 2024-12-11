using Npgsql;

public interface IAccountManager
{
    Task Create(NpgsqlConnection connection, User user);
    Task CheckUsername(NpgsqlConnection connection, string username);
}
