using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;

public class LoginMenu : Menu
{
    public LoginMenu(GetManagers getManagers)
    {
        AddCommand(new CreateAccountCommand(getManagers));
        AddCommand(new LoginCommand(getManagers));
        // AddCommand(new GuestCommand(getManagers));
        AddCommand(new ExitCommand(getManagers));
        AddCommand(new LoginAdminCommand(getManagers));
    }

    public override void Display()
    {
        Console.Clear();

        Console.WriteLine("Login Menu:\n");

        ChangeColor.TextColorGreen("[C]");
        Console.WriteLine("reate Account");
        ChangeColor.TextColorGreen("[L]");
        Console.WriteLine("ogin");
        ChangeColor.TextColorGreen("[G]");
        Console.WriteLine("uest Account");
        ChangeColor.TextColorGreen("[E]");
        Console.WriteLine("xit");

        if (DateTime.Now.Day % 2 == 0)
        {
            Console.WriteLine("\nThere is currently a sign up bonus,\nsign up today and gain a 10 000:-");
        }
    }
}
