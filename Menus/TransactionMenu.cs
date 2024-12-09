using Individuell_Uppgift.Utilities;
using Npgsql;

namespace Individuell_Uppgift.Menus;

public class TransactionMenu : Menu
{
    public TransactionMenu(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
    {
        AddCommand(new AddTransactionCommand(connection, accountManager, menuManager, transactionManager));
        AddCommand(new DeleteTransactionCommand(connection, accountManager, menuManager, transactionManager));
        AddCommand(new CheckBalanceCommand(connection, accountManager, menuManager, transactionManager));
        AddCommand(new CheckIncomeCommand(connection, accountManager, menuManager, transactionManager));
        AddCommand(new CheckExpenseCommand(connection, accountManager, menuManager, transactionManager));
        AddCommand(new LogoutCommand(connection, accountManager, menuManager, transactionManager));
        AddCommand(new HelpCommand(connection, accountManager, menuManager, transactionManager));
        AddCommand(new CheckAllTransactionsCommand(connection, accountManager, menuManager, transactionManager));
    }

    public override void Display()
    {
        Console.Clear();

        ChangeColor.TextColorGreen("[A]");
        Console.WriteLine("dd Transaction");
        ChangeColor.TextColorGreen("[D]");
        Console.WriteLine("elete Transaction");
        ChangeColor.TextColorGreen("[B]");
        Console.WriteLine("alance");
        ChangeColor.TextColorGreen("[I]");
        Console.WriteLine("ncome Summary");
        ChangeColor.TextColorGreen("[E]");
        Console.WriteLine("xpense Summary");
        ChangeColor.TextColorGreen("[L]");
        Console.WriteLine("og Out");
        ChangeColor.TextColorGreen("[H]");
        Console.WriteLine("elp Page");
    }

    // Remove later
    public static void Execute()
    {
        Console.Clear();

        ChangeColor.TextColorGreen("[A]");
        Console.WriteLine("dd Transaction");
        ChangeColor.TextColorGreen("[D]");
        Console.WriteLine("elete Transaction");
        ChangeColor.TextColorGreen("[B]");
        Console.WriteLine("alance");
        ChangeColor.TextColorGreen("[I]");
        Console.WriteLine("ncome Summary");
        ChangeColor.TextColorGreen("[E]");
        Console.WriteLine("xpense Summary");
        ChangeColor.TextColorGreen("[L]");
        Console.WriteLine("og Out");
        ChangeColor.TextColorGreen("[H]");
        Console.WriteLine("elp Page");
    }
}
