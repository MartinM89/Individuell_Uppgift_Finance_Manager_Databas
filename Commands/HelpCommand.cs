using Npgsql;

public class HelpCommand : Command
{
    public HelpCommand(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
        : base("H", connection, accountManager, menuManager, transactionManager) { }

    public override string GetDescription()
    {
        return "Check help commands";
    }

    public override Task Execute()
    {
        throw new NotImplementedException();
    }
}
