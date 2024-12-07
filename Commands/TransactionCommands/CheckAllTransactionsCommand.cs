using Npgsql;

public class CheckAllTransactionsCommand : Command
{
    NpgsqlConnection connection;

    public CheckAllTransactionsCommand(NpgsqlConnection connection)
        : base(connection, "P")
    {
        this.connection = connection;
    }

    public override string GetDescription()
    {
        return "Print a list of all transactions";
    }

    public override async Task Execute()
    {
        Console.WriteLine("List of all transactions:");

        PostgresTransactionManager postgresTransactionManager = new(connection);
        List<Transaction> transactions = await postgresTransactionManager.GetAllTransactions();

        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetMultipleRowsTransactionTableCenter(transactions);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();
    }
}
