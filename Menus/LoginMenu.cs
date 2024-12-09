using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;
using Npgsql;

public class LoginMenu : Menu
{
    public LoginMenu(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
    {
        AddCommand(new CreateAccountCommand(connection, accountManager, menuManager, transactionManager));
        AddCommand(new LoginCommand(connection, accountManager, menuManager, transactionManager));
        // AddCommand(new GuestCommand(connection, accountManager, menuManager, transactionManager));
        AddCommand(new ExitCommand(connection, accountManager, menuManager, transactionManager));
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
