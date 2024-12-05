using Individuell_Uppgift.Utilities;

namespace Individuell_Uppgift.Menus;

public class TransactionMenu
{
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
