using Npgsql;

public class CheckAllTransactionsCommand : Command
{
    public CheckAllTransactionsCommand(GetManagers getManagers)
        : base("P", getManagers) { }

    public override string GetDescription()
    {
        return "Print a list of all transactions";
    }

    public override async Task Execute()
    {
        Console.WriteLine("List of all transactions:");

        PostgresTransactionManager postgresTransactionManager = new(GetManagers.Connection);
        List<Transaction> transactions = await postgresTransactionManager.GetAllTransactions();

        Console.Clear();

        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetMultipleRowsTransactionTableCenter(transactions);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
