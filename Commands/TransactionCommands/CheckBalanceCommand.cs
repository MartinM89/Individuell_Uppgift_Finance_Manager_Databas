using Individuell_Uppgift.Utilities;
using Npgsql;

public class CheckBalanceCommand : Command
{
    public CheckBalanceCommand(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
        : base("B", connection, accountManager, menuManager, transactionManager) { }

    public override string GetDescription()
    {
        return "Check your current balance";
    }

    public override async Task Execute()
    {
        PostgresTransactionManager postgresTransactionManager = new(connection);

        decimal totalAmount = await postgresTransactionManager.GetBalance();

        Console.Clear();

        Console.Write("Your total balance is ");
        ChangeColor.TextColorGreen($"{totalAmount:N2}");
        Console.WriteLine(".");

        PressKeyToContinue.Execute();

        menuManager.ReturnToSameMenu();
    }
}
