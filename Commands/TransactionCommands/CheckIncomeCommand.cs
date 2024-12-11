using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;
using Npgsql;

public class CheckIncomeCommand : Command
{
    public CheckIncomeCommand(GetManagers getManagers)
        : base("I", getManagers) { }

    public override string GetDescription()
    {
        return "Check your income";
    }

    public override async Task Execute()
    {
        Console.Clear();

        PostgresTransactionManager getTransaction = new(GetManagers.Connection);

        // int transactionCount = TransactionManager.GetTransactionCount();  // Implement if table is empty for Guid

        // if (transactionCount == 0)
        // {
        //     Console.WriteLine("There are no saved transactions.");
        //     PressKeyToContinue.Execute();
        //     break;
        // }

        CheckIncomeAndExpenseMenu checkIncomeMenu = new();

        checkIncomeMenu.Display();

        string userChoice = string.Empty;
        string hideUserChoice = HideCursor.Execute(userChoice).ToUpper();
        // csharpier-ignore
        if (string.IsNullOrEmpty(hideUserChoice)) { return; }

        if (!hideUserChoice.All(Char.IsLetter) && !hideUserChoice.Length.Equals(1))
        {
            Console.Clear();
            Console.WriteLine("Invalid Input. [DWMY]");
            PressKeyToContinue.Execute();
            return;
        }

        (TransactionCategory, Func<int, bool, Task<List<Transaction>>>) values = hideUserChoice switch
        {
            "D" => (TransactionCategory.Day, getTransaction.GetTransactionsByDay),
            "W" => (TransactionCategory.Week, getTransaction.GetTransactionsByWeek),
            "M" => (TransactionCategory.Month, getTransaction.GetTransactionsByMonth),
            "Y" => (TransactionCategory.Year, getTransaction.GetTransactionsByYear),
            _ => (TransactionCategory.Null, null!),
        };

        var (transactionCategory, fetchTransactions) = values;

        Console.CursorVisible = true;

        Console.Clear();
        Console.Write($"What {transactionCategory} do you wish to check? ");
        if (int.TryParse(Console.ReadLine()!, out int transactionDate)) { }

        Console.CursorVisible = false;

        bool transactionType = true;
        List<Transaction> transactions = await fetchTransactions(transactionDate, transactionType);
        // // csharpier-ignore
        // if (hideUserChoice.Equals("D")) { transactionCategory = TransactionCategory.Day; }
        // else if (hideUserChoice.Equals("W")) { transactionCategory = TransactionCategory.Week; }
        // else if (hideUserChoice.Equals("M")) { transactionCategory = TransactionCategory.Month; }
        // else if (hideUserChoice.Equals("Y")) { transactionCategory = TransactionCategory.Year; }

        Console.Clear();

        Console.WriteLine($"{transactionCategory}:");
        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetMultipleRowsTransactionTableCenter(transactions);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
