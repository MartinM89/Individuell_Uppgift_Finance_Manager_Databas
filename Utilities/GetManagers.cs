using Npgsql;

public class GetManagers
{
    public NpgsqlConnection Connection { get; private set; }
    public IAccountManager AccountManager { get; private set; }
    public ITransactionManager TransactionManager { get; private set; }
    public IMenuManager UserMenuManager { get; private set; }

    public GetManagers(NpgsqlConnection connection, IAccountManager accountManager, ITransactionManager transactionManager, IMenuManager userMenuManager)
    {
        Connection = connection;
        AccountManager = accountManager;
        TransactionManager = transactionManager;
        UserMenuManager = userMenuManager;
    }
}
