using Npgsql;
using Npgsql.Replication;

public class Managers
{
    public NpgsqlConnection Connection { get; private set; }
    public IAccountManager AccountManager { get; private set; }
    public IMenuManager UserMenuManager { get; private set; }
    public ITransactionManager TransactionManager { get; private set; }
    private readonly Managers managers;

    public Managers(NpgsqlConnection connection, IAccountManager accountManager, ITransactionManager transactionManager, IMenuManager userMenuManager)
    {
        Connection = connection;
        AccountManager = accountManager;
        UserMenuManager = userMenuManager;
        TransactionManager = transactionManager;
    }

    public static void Initialize(NpgsqlConnection connection)
    {
        IAccountManager accountManager = new PostgresAccountManager(connection);
        UserMenuManager userMenuManager = new(); // IMenuManager userMenuManager = new UserMenuManager();
        ITransactionManager transactionManager = new PostgresTransactionManager(connection);
        Managers managers = new(connection, accountManager, transactionManager, userMenuManager);
    }
}
