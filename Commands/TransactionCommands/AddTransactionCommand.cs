using Npgsql;

public class AddTransactionCommand : Command
{
    public AddTransactionCommand(GetManagers getManagers)
        : base("A", getManagers) { }

    public override string GetDescription()
    {
        return "Adds a transaction";
    }

    public override async Task Execute()
    {
        Console.Clear();
        // csharpier-ignore
        Transaction transaction = new(
            1,
            GetTransactionName.Execute(),
            GetTransactionAmount.Execute(),
            DateTime.Now,
            PostgresAccountManager.LoggedInUserId);

        PostgresTransactionManager postgresTransactionManager = new(GetManagers.Connection);

        await postgresTransactionManager.AddTransaction(transaction);

        Console.Clear();
        Console.WriteLine("The following transaction has been added:");
        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetSingleRowTransactionTableCenter(transaction);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
