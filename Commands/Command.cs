using Npgsql;

public abstract class Command
{
    public string Name { get; init; }
    protected NpgsqlConnection connection;
    protected IAccountManager accountManager;
    protected IMenuManager menuManager;
    protected ITransactionManager transactionManager;

    public Command(string name, NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
    {
        this.Name = name;
        this.connection = connection;
        this.accountManager = accountManager;
        this.menuManager = menuManager;
        this.transactionManager = transactionManager;
    }

    public abstract Task Execute();

    public abstract string GetDescription();
}
