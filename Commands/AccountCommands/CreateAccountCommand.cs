using Individuell_Uppgift.Utilities;

public class CreateAccountCommand : Command
{
    public CreateAccountCommand(GetManagers getManagers)
        : base('C', "Create", getManagers) { }

    public override string GetDescription()
    {
        return "Create a new account";
    }

    public override void Execute()
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

        bool usernameExists = GetManagers.AccountManager.CheckIfUsernameRegistered(username);

        if (usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Username unavailable.\n");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        Console.Write("Enter password: ");
        password = HidePassword.Execute();

        if (string.IsNullOrEmpty(password))
        {
            GetManagers.UserMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        Console.Write("\nRetype password: ");
        confirmPassword = HidePassword.Execute();

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

        PasswordHasher.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = new(username, passwordHash, passwordSalt);

        GetManagers.AccountManager.Create(user);

        Console.Clear();
        ChangeColor.TextColorGreen($"Account {username} registered successfully.\n");
        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
