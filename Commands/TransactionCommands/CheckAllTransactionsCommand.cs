using Npgsql;

public class CheckAllTransactionsCommand : Command
{
    public CheckAllTransactionsCommand()
        : base("Check All Transactions") { }

    public override string GetDescription()
    {
        return "Print a list of all transactions";
    }

    public override async Task Execute(NpgsqlConnection connection)
    {
        Console.WriteLine("List of all transactions:");

        PostgresTransactionManager postgresTransactionManager = new(connection);
        List<Transaction> transactions = await postgresTransactionManager.GetAllTransactions();

        Console.WriteLine(" _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _");
        Console.WriteLine("| Id  | Date        | Transaction Name                |      Amount |");
        Console.WriteLine("|‾ ‾ ‾|‾ ‾ ‾ ‾ ‾ ‾ ‾|‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾|‾ ‾ ‾ ‾ ‾ ‾ ‾|");

        foreach (Transaction transaction in transactions)
        {
            Console.WriteLine(transaction);
        }

        Console.WriteLine("|_ _ _|_ _ _ _ _ _ _|_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _|_ _ _ _ _ _ _|");

        PressKeyToContinue.Execute();
    }
}
