using Npgsql;

public class LogoutCommand : Command
{
    public LogoutCommand(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
        : base("L", connection, accountManager, menuManager, transactionManager) { }

    public override string GetDescription()
    {
        return "Log out from current user";
    }

    public override Task Execute()
    {
        Console.Clear();
        Console.WriteLine("Thank you for using your personal finance app.");
        PressKeyToContinue.Execute();

        menuManager.SetMenu(new LoginMenu(connection, accountManager, menuManager, transactionManager));

        return Task.CompletedTask;
    }
}
