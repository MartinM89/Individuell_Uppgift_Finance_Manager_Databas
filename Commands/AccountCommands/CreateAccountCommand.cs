using Individuell_Uppgift.Utilities;

public class CreateAccountCommand : Command
{
    public CreateAccountCommand(GetManagers getManagers)
        : base('C', "Create", getManagers) { }

    public override string GetDescription()
    {
        return "Create a new account";
    }

    public override async Task Execute()
    {
        Console.Clear();

        string password;
        string confirmPassword;

        Console.WriteLine("Create Account Menu:\n");

        Console.CursorVisible = true;
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        Console.CursorVisible = false;

        if (string.IsNullOrEmpty(username))
        {
            GetManagers.UserMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        username = username[..1].ToUpper() + username[1..].ToLower();

        bool usernameExists = await GetManagers.AccountManager.CheckIfUsernameRegistered(username);

        if (usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Username unavailable.\n");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        Console.Write("Enter password: ");
        password = HideCursor.Password();

        if (string.IsNullOrEmpty(password))
        {
            GetManagers.UserMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        Console.Write("\nRetype password: ");
        confirmPassword = HideCursor.Password();

        if (string.IsNullOrEmpty(confirmPassword))
        {
            GetManagers.UserMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        if (!password.Equals(confirmPassword))
        {
            Console.Clear();
            ChangeColor.TextColorRed("Passwords do not match.\n");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        (byte[] passwordHash, byte[] passwordSalt) = PasswordHasher.CreatePasswordHash(password);

        User user = new(username, passwordHash, passwordSalt);

        if (DateTime.Now.Day % 2 == 0)
        {
            await GetManagers.AccountManager.CreateUserBonusReward(user);
        }
        else
        {
            await GetManagers.AccountManager.CreateUser(user);
        }

        Console.Clear();
        ChangeColor.TextColorGreen($"Account {username} registered successfully.\n");
        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
