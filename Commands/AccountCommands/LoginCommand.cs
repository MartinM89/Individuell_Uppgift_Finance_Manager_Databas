using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;

public class LoginCommand : Command
{
    public LoginCommand(GetManagers getManagers)
        : base("L", getManagers) { }

    public override string GetDescription()
    {
        return "Use to login in";
    }

    public override async Task Execute()
    {
        Console.Clear();

        string enteredPassword = string.Empty;

        Console.WriteLine("Login Menu:\n");

        Console.Write("Enter username: ");
        string username = Console.ReadLine()!;

        if (string.IsNullOrEmpty(username))
        {
            Console.WriteLine("Fel");
            PressKeyToContinue.Execute();
            return;
        }

        username = username[..1].ToUpper() + username[1..].ToLower();

        bool usernameExists = UserNameUnavailable.Execute(GetManagers.Connection, username);

        if (!usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Could not find account.\n");
            PressKeyToContinue.Execute();
            return;
        }

        Console.Write("Enter password: ");
        enteredPassword = HidePassword.Execute(enteredPassword);

        if (string.IsNullOrEmpty(enteredPassword))
        {
            Console.WriteLine("Fel");
            PressKeyToContinue.Execute();
            return;
        }

        bool isPasswordCorrect = await PostgresAccountManager.CheckLoginDetailsIsCorrect(GetManagers.Connection, username, enteredPassword);

        if (!isPasswordCorrect)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Password does not match.\n");
            PressKeyToContinue.Execute();
            return;
        }

        await GetManagers.AccountManager.CheckUsername(GetManagers.Connection, username);

        Console.Clear();
        ChangeColor.TextColorGreen($"Login successful as {username}.\n");
        while (Console.KeyAvailable)
        {
            Console.ReadKey(true);
        }
        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.SetMenu(new TransactionMenu(GetManagers));
    }
}
