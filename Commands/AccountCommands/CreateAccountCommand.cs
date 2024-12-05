using Individuell_Uppgift.Utilities;
using Npgsql;

public class CreateAccountCommand : Command
{
    public CreateAccountCommand()
        : base("Create Account") { }

    public override string GetDescription()
    {
        return "Create a new account";
    }

    public override Task Execute(NpgsqlConnection connection)
    {
        PostgresAccountManager postgresAccountManager = new();

        Console.Clear();

        string password = string.Empty;
        string confirmPassword = string.Empty;

        Console.Write("Enter username: ");
        string username = Console.ReadLine()!;

        if (string.IsNullOrEmpty(username))
        {
            return Task.CompletedTask;
        }

        username = username[..1].ToUpper() + username[1..].ToLower();

        bool usernameExists = UserNameUnavailable.Execute(connection, username);

        if (usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Username unavailable.\n");
            PressKeyToContinue.Execute();
            return Task.CompletedTask;
        }

        Console.Write("Enter password: ");
        password = HidePassword.Execute(password);

        if (string.IsNullOrEmpty(password))
        {
            return Task.CompletedTask;
        }

        Console.Write("\nRetype password: ");
        confirmPassword = HidePassword.Execute(confirmPassword);

        if (string.IsNullOrEmpty(confirmPassword))
        {
            return Task.CompletedTask;
        }

        if (!password.Equals(confirmPassword))
        {
            Console.Clear();
            ChangeColor.TextColorRed("Passwords do not match.\n");
            PressKeyToContinue.Execute();
            return Task.CompletedTask;
        }

        PasswordHasher.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = new User(username, passwordHash, passwordSalt);

        postgresAccountManager.Create(connection, user);

        Console.Clear();
        ChangeColor.TextColorGreen($"Account {username} registered successfully.\n");
        PressKeyToContinue.Execute();
        return Task.CompletedTask;
    }
}
