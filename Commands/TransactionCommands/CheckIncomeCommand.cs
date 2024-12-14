using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;
using Npgsql;

public class CheckIncomeCommand : Command
{
    private readonly string[] availableInputs = ["D", "W", "M", "Y"];

    public CheckIncomeCommand(GetManagers getManagers)
        : base('I', "Income", getManagers) { }

    public override string GetDescription()
    {
        return "Check your income";
    }

    public override void Execute()
    {
        Console.Clear();

        // int transactionCount = TransactionManager.GetTransactionCount();  // Implement if table is empty for Guid

        // if (transactionCount == 0)
        // {
        //     Console.WriteLine("There are no saved transactions.");
        //     PressKeyToContinue.Execute();
        //     break;
        // }

        CheckIncomeAndExpenseMenu checkIncomeMenu = new(); // Check

        checkIncomeMenu.Display();

        string hideUserChoice = HideCursor.Execute().ToUpper();

        if (string.IsNullOrEmpty(hideUserChoice))
        {
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        if (!availableInputs.Contains(hideUserChoice))
        {
            Console.Clear();
            Console.WriteLine("Invalid Input. [DWMY]");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        (TransactionCategory, Func<int, bool, List<Transaction>>) values = hideUserChoice switch
        {
            "D" => (TransactionCategory.Day, GetManagers.TransactionManager.GetTransactionsByDay),
            "W" => (TransactionCategory.Week, GetManagers.TransactionManager.GetTransactionsByWeek),
            "M" => (TransactionCategory.Month, GetManagers.TransactionManager.GetTransactionsByMonth),
            "Y" => (TransactionCategory.Year, GetManagers.TransactionManager.GetTransactionsByYear),
            _ => (TransactionCategory.Null, null!),
        };

        var (transactionCategory, fetchTransactions) = values;

        Console.CursorVisible = true;

        Console.Clear();
        Console.Write($"What {transactionCategory} do you wish to check? ");
        _ = int.TryParse(Console.ReadLine(), out int transactionDate);

        Console.CursorVisible = false;

        bool transactionType = true;
        List<Transaction> transactions = fetchTransactions(transactionDate, transactionType);
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
