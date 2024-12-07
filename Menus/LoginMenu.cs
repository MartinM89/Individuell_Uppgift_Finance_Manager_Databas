using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;
using Npgsql;

public class LoginMenu : Menu
{
    NpgsqlConnection connection;

    public LoginMenu(NpgsqlConnection connection)
    {
        this.connection = connection;
        AddCommand(new LoginCommand(connection));
        AddCommand(new CreateAccountCommand(connection));
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
