using Individuell_Uppgift.Utilities;

namespace Individuell_Uppgift.Menus;

public class AccountMenu
{
    public static void Execute()
    {
        Console.Clear();

        ChangeColor.TextColorGreen("[C]");
        Console.WriteLine("reate Account");
        ChangeColor.TextColorGreen("[L]");
        Console.WriteLine("ogin");
        ChangeColor.TextColorGreen("[G]");
        Console.WriteLine("uest Account");
        ChangeColor.TextColorGreen("[E]");
        Console.WriteLine("xit");
    }
}
