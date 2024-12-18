using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;

public class TransactionMenuAdmin : Menu
{
    public TransactionMenuAdmin(GetManagers getManagers)
    {
        AddCommand(new AddTransactionCommand(getManagers));
        AddCommand(new DeleteTransactionCommand(getManagers));
        AddCommand(new CheckBalanceCommand(getManagers));
        AddCommand(new CheckIncomeCommand(getManagers));
        AddCommand(new CheckExpenseCommand(getManagers));
        AddCommand(new LogoutCommand(getManagers));
        AddCommand(new HelpCommand(getManagers));
        AddCommand(new CheckAllTransactionsCommand(getManagers));
    }

    public override void Display()
    {
        Console.Clear();

        Console.WriteLine("Admin Transasction Menu:\n");

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
