using Npgsql;

public interface IAccount
{
    void Create(NpgsqlConnection connection, User user);
    void Login(NpgsqlConnection connection, string username);
}
