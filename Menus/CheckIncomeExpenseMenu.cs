using Individuell_Uppgift.Utilities;

namespace Individuell_Uppgift.Menus;

public class CheckIncomeExpenseMenu
{
    public static void Execute()
    {
        Console.WriteLine("What do you wish to see:\n");
        ChangeColor.TextColorGreen("[D]");
        Console.WriteLine("aily");
        ChangeColor.TextColorGreen("[W]");
        Console.WriteLine("eekly");
        ChangeColor.TextColorGreen("[M]");
        Console.WriteLine("onthly");
        ChangeColor.TextColorGreen("[Y]");
        Console.WriteLine("early");
    }
}
