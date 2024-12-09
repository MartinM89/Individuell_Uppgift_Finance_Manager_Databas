using Individuell_Uppgift.Utilities;
using Npgsql;

public class CreateAccountCommand : Command
{
    public CreateAccountCommand(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
        : base("C", connection, accountManager, menuManager, transactionManager) { }

    public override string GetDescription()
    {
        return "Create a new account";
    }

    public override async Task Execute()
    {
        PostgresAccountManager postgresAccountManager = new(connection);

        Console.Clear();

        string password = string.Empty;
        string confirmPassword = string.Empty;

        Console.Write("Enter username: ");
        string username = Console.ReadLine()!;

        if (string.IsNullOrEmpty(username))
        {
            return;
        }

        username = username[..1].ToUpper() + username[1..].ToLower();

        bool usernameExists = UserNameUnavailable.Execute(connection, username);

        if (usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Username unavailable.\n");
            PressKeyToContinue.Execute();
            return;
        }

        Console.Write("Enter password: ");
        password = HidePassword.Execute(password);

        if (string.IsNullOrEmpty(password))
        {
            return;
        }

        Console.Write("\nRetype password: ");
        confirmPassword = HidePassword.Execute(confirmPassword);

        if (string.IsNullOrEmpty(confirmPassword))
        {
            return;
        }

        if (!password.Equals(confirmPassword))
        {
            Console.Clear();
            ChangeColor.TextColorRed("Passwords do not match.\n");
            PressKeyToContinue.Execute();
            return;
        }

        PasswordHasher.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = new User(username, passwordHash, passwordSalt);

        await postgresAccountManager.Create(connection, user);

        Console.Clear();
        ChangeColor.TextColorGreen($"Account {username} registered successfully.\n");
        PressKeyToContinue.Execute();
        return;
    }
}
