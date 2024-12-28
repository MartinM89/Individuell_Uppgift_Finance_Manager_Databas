public class CheckAllTransactionsCommand : Command
{
    public CheckAllTransactionsCommand(GetManagers getManagers)
        : base('P', "All", getManagers) { }

    public override string GetDescription()
    {
        return "Print a list of all transactions";
    }

    public override void Execute() // Only for debuggin
    {
        Console.WriteLine("List of all transactions:");

        List<Transaction> transactions = GetManagers.TransactionManager.GetAllTransactions(PostgresAccountManager.GetLoggedInUserId());

        if (transactions.Count.Equals(0))
        {
            Console.WriteLine("There are no saved transactions.");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetMultipleRowsTransactionTableCenter(transactions);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
