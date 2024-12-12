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
        Console.Clear();

        List<Transaction> transactions = await GetManagers.TransactionManager.GetAllTransactions();

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

        int rowsAffected = await GetManagers.TransactionManager.DeleteTransaction(transactionToDelete);

        if (rowsAffected <= 0)
        {
            Console.Clear();
            ChangeColor.TextColorRed($"{transactionToDelete} does not exists.\n");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        Console.Clear();

        Console.WriteLine($"Transaction {transactionToDelete} deleted.");

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
