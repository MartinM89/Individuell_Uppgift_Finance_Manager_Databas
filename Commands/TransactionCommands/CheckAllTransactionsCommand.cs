public class CheckAllTransactionsCommand : Command
{
    public CheckAllTransactionsCommand(GetManagers getManagers)
        : base('P', "All", getManagers) { }

    public override string GetDescription()
    {
        return "Print a list of all transactions";
    }

    public override void Execute()
    {
        Console.WriteLine("List of all transactions:");

        List<Transaction> transactions = GetManagers.TransactionManager.GetAllTransactions();

        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetMultipleRowsTransactionTableCenter(transactions);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
