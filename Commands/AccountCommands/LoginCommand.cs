using Npgsql;

public class LoginCommand : Command
{
    public LoginCommand()
        : base("Login") { }

    public override string GetDescription()
    {
        return "Use to login in";
    }

    public override void Execute(NpgsqlConnection connection)
    {
        PostgresAccountManager postgresAccountManager = new();
        Console.Clear();

        string enteredPassword = string.Empty;

        Console.Write("Enter username: ");
        string username = Console.ReadLine()!;

        if (string.IsNullOrEmpty(username))
        {
            return;
        }

        username = username[..1].ToUpper() + username[1..].ToLower();

        bool usernameExists = UserNameUnavailable.Execute(connection, username); // Unnecessary?

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
            return;
        }

        bool isPasswordCorrect = PostgresAccountManager.CheckLoginDetailsIsCorrect(connection, username, enteredPassword);

        if (!isPasswordCorrect)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Password does not match.\n");
            PressKeyToContinue.Execute();
            return;
        }

        postgresAccountManager.Login(connection, username);

        Console.Clear();
        ChangeColor.TextColorGreen($"Login successful as {username}.\n");
        PressKeyToContinue.Execute();
    }
}
