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

        Console.Write("What transaction do you wish to delete? ");
        int transactionToDelete = int.Parse(Console.ReadLine()!);

        PostgresTransactionManager postgresTransactionManager = new(connection);

        await postgresTransactionManager.DeleteTransaction(transactionToDelete);

        Console.Clear();
        Console.WriteLine($"Transaction {transactionToDelete} deleted.");
        PressKeyToContinue.Execute();
    }
}
