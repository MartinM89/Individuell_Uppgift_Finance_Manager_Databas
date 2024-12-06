using Npgsql;

public class DeleteTransactionCommand : Command
{
    public DeleteTransactionCommand()
        : base("Delete Transaction") { }

    public override string GetDescription()
    {
        return "Delete a transaction";
    }

    public override async Task Execute(NpgsqlConnection connection)
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

        int rowCount = await postgresTransactionManager.GetTransactionsCount();

        if (transactionToDelete >= rowCount)
        {
            Console.WriteLine("Transaction does not exist.");
            PressKeyToContinue.Execute();
            return;
        }

        await postgresTransactionManager.DeleteTransaction(transactionToDelete);

        Console.Clear();
        Console.WriteLine($"Transaction {transactionToDelete} deleted.");
        PressKeyToContinue.Execute();
    }
}
