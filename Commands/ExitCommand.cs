using Npgsql;

public class ExitCommand : Command
{
    public ExitCommand(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
        : base("E", connection, accountManager, menuManager, transactionManager) { }

    public override string GetDescription()
    {
        return "Exit the program";
    }

    public override Task Execute()
    {
        throw new NotImplementedException();
    }
}
