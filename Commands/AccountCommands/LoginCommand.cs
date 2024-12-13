using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;

public class LoginCommand : Command
{
    public LoginCommand(GetManagers getManagers)
        : base('L', "Login", getManagers) { }

    public override string GetDescription()
    {
        return "Use to login in";
    }

    public override async Task Execute()
    {
        Console.Clear();

        var connection = GetManagers.Connection;
        var userMenuManager = GetManagers.UserMenuManager;
        var accountManager = GetManagers.AccountManager;

        string enteredPassword = string.Empty;

        Console.WriteLine("Login Menu:\n");

        Console.Write("Enter username: ");
        string username = Console.ReadLine()!;

        if (string.IsNullOrEmpty(username))
        {
            userMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        username = username[..1].ToUpper() + username[1..].ToLower();

        bool usernameExists = accountManager.CheckUsernameRegistered(connection, username);

        if (!usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Could not find account.\n");
            PressKeyToContinue.Execute();
            userMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        Console.Write("Enter password: ");
        enteredPassword = HidePassword.Execute(enteredPassword);

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

        await GetManagers.AccountManager.GetUserGuid(GetManagers.Connection, username);

        Console.Clear();
        ChangeColor.TextColorGreen($"Login successful as {username}.\n");
        // SimulateKeyPress.Execute();
        PressKeyToContinue.Execute();

        userMenuManager.SetMenu(new TransactionMenu(GetManagers));
    }
}
