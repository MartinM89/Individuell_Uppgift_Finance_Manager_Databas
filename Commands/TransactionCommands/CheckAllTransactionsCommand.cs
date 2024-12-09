using Individuell_Uppgift.Menus;
using Npgsql;

public class CheckAllTransactionsCommand : Command
{
    public CheckAllTransactionsCommand(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
        : base("P", connection, accountManager, menuManager, transactionManager) { }

    public override string GetDescription()
    {
        return "Print a list of all transactions";
    }

    public override async Task Execute()
    {
        Console.WriteLine("List of all transactions:");

        PostgresTransactionManager postgresTransactionManager = new(connection);
        List<Transaction> transactions = await postgresTransactionManager.GetAllTransactions();

        Console.Clear();

        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetMultipleRowsTransactionTableCenter(transactions);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        // menuManager.SetMenu(new TransactionMenu(connection, accountManager, menuManager, transactionManager));

        menuManager.ReturnToSameMenu();
    }
}
