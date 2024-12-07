using Npgsql;

public interface IAccountManager
{
    Task Create(NpgsqlConnection connection, User user);
    Task Login(NpgsqlConnection connection, string username);
}
