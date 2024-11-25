using Npgsql;

public class CreateAccountCommand : Command
{
    private NpgsqlConnection? connection;

    public CreateAccountCommand()
        : base("Create Account") { }

    public override string GetDescription()
    {
        return "Create a new account";
    }

    public override void RunCommand()
    {
        Console.Clear();

        string connectionString = DatabaseConnection.GetConnectionString();

        this.connection = new NpgsqlConnection(connectionString);
        connection.Open();

        string password = string.Empty;
        string confirmPassword = string.Empty;

        Console.Write("Enter username: ");
        string username = Console.ReadLine()!;

        if (string.IsNullOrEmpty(username))
        {
            return;
        }

        username = username[..1].ToUpper() + username[1..].ToLower();

        bool usernameExists = UserNameUnavailable.Execute(username, connection);

        if (usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("That username is unavailable.\n");
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

        var createAccountSql = """
            INSERT INTO users (username, password_hash, password_salt)
            VALUES (@username, @password_hash, @password_salt)
            """;

        var command = new NpgsqlCommand(createAccountSql, connection);
        command.Parameters.AddWithValue("username", username);
        command.Parameters.AddWithValue("password_hash", Convert.ToBase64String(passwordHash));
        command.Parameters.AddWithValue("password_salt", Convert.ToBase64String(passwordSalt));

        command.ExecuteNonQuery();

        Console.Clear();
        ChangeColor.TextColorGreen($"Account {username} registered successfully.\n");
        PressKeyToContinue.Execute();

        connection.Close();
    }
}
