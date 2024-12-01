using Npgsql;

public class UserAccount : IAccount
{
    readonly CreateAccountCommand create = new();
    readonly LoginCommand login = new();

    public void Create(NpgsqlConnection connection)
    {
        create.RunCommand(connection);
    }

    public Guid GetUserId(Guid id)
    {
        return id;
    }

    public void Login(NpgsqlConnection connection)
    {
        login.RunCommand(connection);
    }
}
