public class AddTransactionCommand : Command
{
    public AddTransactionCommand(GetManagers getManagers)
        : base('A', "Add", getManagers) { }

    public override string GetDescription()
    {
        return "Adds a transaction";
    }

    public override void Execute()
    {
        Console.Clear();

        string? name = GetTransactionName.Execute();

        if (string.IsNullOrEmpty(name))
        {
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        decimal amount = GetTransactionAmount.Execute();

        if (amount.Equals(0))
        {
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        Transaction transaction = new(1, name, amount, DateTime.Now, PostgresAccountManager.GetLoggedInUserId());

        GetManagers.TransactionManager.AddTransaction(transaction);

        Console.Clear();
        Console.WriteLine("The following transaction has been added:");
        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetSingleRowTransactionTableCenter(transaction);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
