using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;
using Npgsql;

public class LoginCommand : Command
{
    public LoginCommand(GetManagers getManagers)
        : base('L', "Login", getManagers) { }

    public override string GetDescription()
    {
        return "Use to login in";
    }

    public override void Execute()
    {
        Console.Clear();

        NpgsqlConnection connection = GetManagers.Connection;
        IMenuManager userMenuManager = GetManagers.UserMenuManager;
        IAccountManager accountManager = GetManagers.AccountManager;

        string enteredPassword;

        Console.WriteLine("Login Menu:\n");

        Console.CursorVisible = true;
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        Console.CursorVisible = false;

        if (string.IsNullOrEmpty(username))
        {
            userMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        username = username[..1].ToUpper() + username[1..].ToLower();

        bool usernameExists = accountManager.CheckIfUsernameRegistered(username);

        if (!usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Could not find account.\n");
            PressKeyToContinue.Execute();
            userMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        Console.Write("Enter password: ");
        enteredPassword = HidePassword.Execute();

        if (string.IsNullOrEmpty(enteredPassword))
        {
            userMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        bool isPasswordCorrect = PostgresAccountManager.CheckLoginDetailsIsCorrect(connection, username, enteredPassword);

        if (!isPasswordCorrect)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Password does not match.\n");
            PressKeyToContinue.Execute();
            userMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        GetManagers.AccountManager.GetUserGuid(username);

        Console.Clear();
        ChangeColor.TextColorGreen($"Login successful as {username}.\n");
        PressKeyToContinue.Execute();

        userMenuManager.SetMenu(new TransactionMenu(GetManagers));
    }
}
