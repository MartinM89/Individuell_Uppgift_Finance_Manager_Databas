using Npgsql;

public interface IAccount
{
    void Create(NpgsqlConnection connection);
    void Login(NpgsqlConnection connection);
    Guid GetUserId(Guid id);
}
