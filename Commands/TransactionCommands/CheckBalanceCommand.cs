using Individuell_Uppgift.Utilities;
using Npgsql;

public class CheckBalanceCommand : Command
{
    public CheckBalanceCommand(GetManagers getManagers)
        : base("B", getManagers) { }

    public override string GetDescription()
    {
        return "Check your current balance";
    }

    public override async Task Execute()
    {
        PostgresTransactionManager postgresTransactionManager = new(GetManagers.Connection);

        decimal totalAmount = await postgresTransactionManager.GetBalance();

        Console.Clear();

        Console.Write("Your total balance is ");
        ChangeColor.TextColorGreen($"{totalAmount:N2}");
        Console.WriteLine(".");

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
