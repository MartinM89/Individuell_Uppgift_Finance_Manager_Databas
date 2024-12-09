using Npgsql;

public class CheckExpenseCommand : Command
{
    public CheckExpenseCommand(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
        : base("E", connection, accountManager, menuManager, transactionManager) { }

    public override string GetDescription()
    {
        return "Check your expenses";
    }

    public override Task Execute()
    {
        throw new NotImplementedException();
    }
}
