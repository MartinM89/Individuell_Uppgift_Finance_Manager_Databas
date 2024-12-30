using Individuell_Uppgift.Utilities;

public class DeleteTransactionCommand : Command
{
    public DeleteTransactionCommand(GetManagers getManagers)
        : base('D', "Delete", getManagers) { }

    public override string GetDescription()
    {
        return "Delete a transaction";
    }

    public override async Task Execute()
    {
        var (userGuid, targetUser, adminLoggedIn) = await GetGuidForAdmin.Execute(GetManagers);

        userGuid = userGuid.Equals(Guid.Empty) ? PostgresAccountManager.GetLoggedInUserId() : userGuid;

        Console.Clear();

        List<Transaction> transactions = await GetManagers.TransactionManager.GetAllTransactions(userGuid);

        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetMultipleRowsTransactionTableCenter(transactions);
        TransactionTable.GetTransactionsTableBottom();

        Console.CursorVisible = true;
        Console.Write("\nTransaction to delete: ");
        string transactionToDeleteString = Console.ReadLine()!;
        Console.CursorVisible = false;

        if (string.IsNullOrEmpty(transactionToDeleteString))
        {
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        _ = int.TryParse(transactionToDeleteString, out int transactionToDelete);

        int rowsAffected = await GetManagers.TransactionManager.DeleteTransaction(userGuid, transactionToDelete);

        if (rowsAffected <= 0)
        {
            Console.Clear();
            ChangeColor.TextColorRed($"{transactionToDelete} does not exist.\n");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        Console.Clear();

        Transaction? deletedTransaction = transactions.Find(t => t.Id.Equals(transactionToDelete));

        if (deletedTransaction == null)
        {
            Console.Clear();
            ChangeColor.TextColorRed($"{transactionToDelete} does not exist.");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        Console.WriteLine("The following transaction has been deleted:");
        TransactionTable.GetTransactionTableTop();
        Console.WriteLine($"| {deletedTransaction.Id} | {deletedTransaction.Date:dd MMM yyyy} | {deletedTransaction.Name, -21} | {deletedTransaction.Amount, 13} |");
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
