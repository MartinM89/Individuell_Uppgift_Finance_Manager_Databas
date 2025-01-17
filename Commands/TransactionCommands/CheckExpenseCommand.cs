using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;

public class CheckExpenseCommand : Command
{
    private readonly string[] availableInputs = ["D", "W", "M", "Y"];

    public CheckExpenseCommand(GetManagers getManagers)
        : base('E', "Expense Summary", getManagers) { }

    public override string GetDescription()
    {
        return "Check your expenses";
    }

    public override async Task Execute()
    {
        var (userGuid, targetUser, adminLoggedIn) = await GetGuidForAdmin.Execute(GetManagers);

        userGuid = userGuid.Equals(Guid.Empty) ? PostgresAccountManager.GetLoggedInUserId() : userGuid;

        Console.Clear();

        List<Transaction> transactions = await GetManagers.TransactionManager.GetAllTransactions(userGuid);

        if (transactions.Count.Equals(0))
        {
            Console.WriteLine("There are no saved transactions.");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        CheckIncomeAndExpenseMenu checkIncomeMenu = new();
        checkIncomeMenu.Display();

        string userChoice = HideCursor.Input().ToUpper();

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

        var transactionCategory = userChoice switch
        {
            "D" => TransactionCategory.Day,
            "W" => TransactionCategory.Week,
            "M" => TransactionCategory.Month,
            "Y" => TransactionCategory.Year,
            _ => TransactionCategory.Null,
        };

        Console.Clear();
        Console.CursorVisible = true;
        Console.Write($"What {transactionCategory} do you wish to check? ");
        _ = int.TryParse(Console.ReadLine(), out int timeValue);
        Console.CursorVisible = false;

        string incomeOrExpense = "<";

        transactions = await GetManagers.TransactionManager.GetTransactionsByTime(userGuid, timeValue, transactionCategory.ToString(), incomeOrExpense);

        Console.Clear();

        Console.WriteLine($"{transactionCategory} {timeValue}:");
        TransactionTable.PrintTransactionTableTop();
        TransactionTable.PrintMultipleRowsTransactionTableCenter(transactions);
        TransactionTable.PrintTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
