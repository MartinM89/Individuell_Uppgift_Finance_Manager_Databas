using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;

public class LoginMenu : Menu
{
    public LoginMenu()
    {
        AddCommand(new LoginCommand());
        AddCommand(new CreateAccountCommand());
    }

    public override void Display()
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
