using Npgsql;

public class DeleteTransactionCommand : Command
{
    NpgsqlConnection connection;

    public DeleteTransactionCommand(NpgsqlConnection connection)
        : base(connection, "D")
    {
        this.connection = connection;
    }

    public override string GetDescription()
    {
        return "Delete a transaction";
    }

    public override async Task Execute()
    {
        Console.Clear();

        PostgresTransactionManager postgresTransactionManager = new(connection);

        List<Transaction> transactions = await postgresTransactionManager.GetAllTransactions();

        foreach (Transaction transaction in transactions)
        {
            Console.WriteLine(transaction);
        }

        Console.Write("What transaction do you wish to delete? ");
        int transactionToDelete = int.Parse(Console.ReadLine()!);
        // csharpier-ignore
        // if (string.IsNullOrEmpty(transactionValueString)) { return; }

        int rowsAffected = await postgresTransactionManager.DeleteTransaction(transactionToDelete);

        Console.WriteLine(rowsAffected);
        PressKeyToContinue.Execute();

        if (rowsAffected <= 0)
        {
            Console.Clear();
            Console.WriteLine("Transactions does not exists.");
            PressKeyToContinue.Execute();
            return;
        }

        Console.Clear();
        Console.WriteLine($"Transaction {transactionToDelete} deleted.");
        PressKeyToContinue.Execute();
    }
}
