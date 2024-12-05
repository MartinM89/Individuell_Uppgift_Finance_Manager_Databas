using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;
using Npgsql;

public class CheckIncomeCommand : Command
{
    public CheckIncomeCommand()
        : base("Check Income") { }

    public override string GetDescription()
    {
        return "Check your income";
    }

    public override async Task Execute(NpgsqlConnection connection)
    {
        Console.Clear();

        PostgresTransactionManager getTransaction = new(connection);

        // int transactionCount = TransactionManager.GetTransactionCount();

        // if (transactionCount == 0)
        // {
        //     Console.WriteLine("There are no saved transactions.");
        //     PressKeyToContinue.Execute();
        //     break;
        // }

        CheckIncomeExpenseMenu.Execute();

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

        Enum transactionCategory = hideUserChoice switch
        {
            "D" => TransactionCategory.Day,
            "W" => TransactionCategory.Week,
            "M" => TransactionCategory.Month,
            "Y" => TransactionCategory.Year,
            _ => transactionCategory = null!,
        };

        Console.CursorVisible = true;

        Console.Clear();
        Console.Write($"What {transactionCategory} do you wish to check? ");
        if (int.TryParse(Console.ReadLine()!, out int transactionDate)) { }

        Console.CursorVisible = false;

        char transactionType = '>';
        List<Transaction> transactions = [];

        Console.Clear();

        transactions = hideUserChoice switch
        {
            "D" => transactions = await getTransaction.GetTransactionsByDay(transactionDate, transactionType),
            "W" => transactions = await getTransaction.GetTransactionsByWeek(transactionDate, transactionType),
            "M" => transactions = await getTransaction.GetTransactionsByMonth(transactionDate, transactionType),
            "Y" => transactions = await getTransaction.GetTransactionsByYear(transactionDate, transactionType),
            _ => transactions = null!,
        };
        // csharpier-ignore
            if (hideUserChoice.Equals("D")) { transactionCategory = TransactionCategory.Day; }
            else if (hideUserChoice.Equals("W")) { transactionCategory = TransactionCategory.Week; }
            else if (hideUserChoice.Equals("M")) { transactionCategory = TransactionCategory.Month; }
            else if (hideUserChoice.Equals("Y")) { transactionCategory = TransactionCategory.Year; }

        Console.WriteLine($"{transactionCategory}:");
        Console.WriteLine(" _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _");
        Console.WriteLine("| Date        | Transaction Name                |      Amount |");
        Console.WriteLine("|‾ ‾ ‾ ‾ ‾ ‾ ‾|‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾|‾ ‾ ‾ ‾ ‾ ‾ ‾|");

        foreach (Transaction transaction in transactions)
        {
            Console.WriteLine(transaction);
        }

        Console.WriteLine("|_ _ _ _ _ _ _|_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _|_ _ _ _ _ _ _|");

        PressKeyToContinue.Execute();
    }
}
