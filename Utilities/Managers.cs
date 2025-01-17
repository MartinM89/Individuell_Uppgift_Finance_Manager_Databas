using Npgsql;

public class Managers
{
    public NpgsqlConnection Connection { get; private set; }
    public IAccountManager AccountManager { get; private set; }
    public IMenuManager UserMenuManager { get; private set; }
    public ITransactionManager TransactionManager { get; private set; }

    public Managers(NpgsqlConnection connection, IAccountManager accountManager, ITransactionManager transactionManager, IMenuManager userMenuManager)
    {
        Connection = connection;
        AccountManager = accountManager;
        UserMenuManager = userMenuManager;
        TransactionManager = transactionManager;
    }
}
