using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;

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

        List<Transaction> transactions = GetManagers.TransactionManager.GetAllTransactions(PostgresAccountManager.GetLoggedInUserId());

        if (transactions.Count.Equals(0))
        {
            Console.WriteLine("There are no saved transactions.");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        CheckIncomeAndExpenseMenu checkIncomeMenu = new();
        checkIncomeMenu.Display();

        string userChoice = HideCursor.Execute().ToUpper();

        if (string.IsNullOrEmpty(userChoice))
        {
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        if (!availableInputs.Contains(userChoice))
        {
            Console.Clear();
            Console.WriteLine("Invalid Input. [DWMY]");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        (TransactionCategory, Func<int, bool, List<Transaction>>) values = userChoice switch
        {
            "D" => (TransactionCategory.Day, GetManagers.TransactionManager.GetTransactionsByDay),
            "W" => (TransactionCategory.Week, GetManagers.TransactionManager.GetTransactionsByWeek),
            "M" => (TransactionCategory.Month, GetManagers.TransactionManager.GetTransactionsByMonth),
            "Y" => (TransactionCategory.Year, GetManagers.TransactionManager.GetTransactionsByYear),
            _ => (TransactionCategory.Null, null!),
        };

        var (transactionCategory, fetchTransactions) = values;

        Console.Clear();
        Console.CursorVisible = true;
        Console.Write($"What {transactionCategory} do you wish to check? ");
        _ = int.TryParse(Console.ReadLine(), out int transactionDate);
        Console.CursorVisible = false;

        bool incomeTransaction = true;
        transactions = fetchTransactions(transactionDate, incomeTransaction);

        Console.Clear();

        Console.WriteLine($"{transactionCategory} {transactionDate}:");
        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetMultipleRowsTransactionTableCenter(transactions);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
