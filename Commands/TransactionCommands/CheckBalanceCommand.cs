using Individuell_Uppgift.Utilities;
using Npgsql;

public class CheckBalanceCommand : Command
{
    NpgsqlConnection connection;

    public CheckBalanceCommand(NpgsqlConnection connection)
        : base(connection, "B")
    {
        this.connection = connection;
    }

    public override string GetDescription()
    {
        return "Check your current balance";
    }

    public override async Task Execute()
    {
        PostgresTransactionManager postgresTransactionManager = new(connection);

        decimal totalAmount = await postgresTransactionManager.GetBalance();

        Console.Write("Your total balance is ");
        ChangeColor.TextColorGreen($"{totalAmount:N2}");
        Console.WriteLine(".");
        PressKeyToContinue.Execute();
    }
}
