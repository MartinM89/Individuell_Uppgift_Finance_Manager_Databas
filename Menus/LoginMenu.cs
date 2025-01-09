using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;

public class LoginMenu : Menu
{
    public LoginMenu(GetManagers getManagers)
    {
        AddCommand(new CreateAccountCommand(getManagers));
        AddCommand(new LoginCommand(getManagers));
        // AddCommand(new GuestCommand(getManagers));
        AddCommand(new HelpCommand(getManagers));
        AddCommand(new ExitCommand(getManagers));
        AddCommand(new LoginAdminCommand(getManagers));
    }

    public override void Display()
    {
        Console.Clear();

        Console.WriteLine("Login Menu:\n");

        foreach (Command command in commands)
        {
            ChangeColor.TextColorGreen($"[{command.Shortcut}]");
            Console.WriteLine(command.Name[1..]);
        }

        if (DateTime.Now.Day % 2 == 0)
        {
            Console.WriteLine("\nThere is currently a sign up bonus,\nsign up today and gain a 10 000:-");
        }
    }

    public override void HelpMenu()
    {
        Console.Clear();

        Console.WriteLine("Help Menu:\n");

        foreach (Command command in commands)
        {
            if (command.Shortcut == 'H' || command.Shortcut == 'Å')
            {
                continue;
            }

            ChangeColor.TextColorGreen($"[{command.Name}]");
            Console.WriteLine($" - {command.GetDescription()}");
        }

        Console.ReadKey();
    }
}
