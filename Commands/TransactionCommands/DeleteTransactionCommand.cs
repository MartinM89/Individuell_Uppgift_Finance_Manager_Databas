using Npgsql;

public class DeleteTransactionCommand : Command
{
    public DeleteTransactionCommand(GetManagers getManagers)
        : base("D", getManagers) { }

    public override string GetDescription()
    {
        return "Delete a transaction";
    }

    public override async Task Execute()
    {
        Console.Clear();

        PostgresTransactionManager postgresTransactionManager = new(GetManagers.Connection);

        List<Transaction> transactions = await postgresTransactionManager.GetAllTransactions();

        foreach (Transaction transaction in transactions)
        {
            Console.WriteLine(transaction);
        }

        Console.Write("What transaction do you wish to delete? ");
        string transactionToDeleteString = Console.ReadLine()!;
        // csharpier-ignore
        if (string.IsNullOrEmpty(transactionToDeleteString)) { return; }

        _ = int.TryParse(transactionToDeleteString, out int transactionToDelete);

        int rowsAffected = await postgresTransactionManager.DeleteTransaction(transactionToDelete);

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

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
