using System.Dynamic;
using Npgsql;

public class LoginCommand : Command
{
    public static Guid id = Guid.Empty;

    private NpgsqlConnection? connection;

    public LoginCommand()
        : base("Login") { }

    public override string GetDescription()
    {
        return "Use to login in";
    }

    public override void RunCommand()
    {
        string connectionString = DatabaseConnection.GetConnectionString();

        this.connection = new NpgsqlConnection(connectionString);
        connection.Open();

        string enteredPassword = string.Empty;

        Console.Clear();

        Console.Write("Enter username: ");
        string username = Console.ReadLine()!;

        if (string.IsNullOrEmpty(username))
        {
            return;
        }

        username = username[..1].ToUpper() + username[1..].ToLower();

        bool usernameExists = UserNameUnavailable.Execute(username, connection);

        if (!usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("That username doesn't exist.\n");
            PressKeyToContinue.Execute();
            return;
        }

        Console.Write("Enter password: ");
        enteredPassword = HidePassword.Execute(enteredPassword);

        if (string.IsNullOrEmpty(enteredPassword))
        {
            return;
        }

        var loginSql = """
            SELECT password_hash, password_salt
            FROM users
            WHERE username = @username
            """;

        var command = new NpgsqlCommand(loginSql, connection);
        command.Parameters.AddWithValue("username", username);

        var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            Console.Clear();
            ChangeColor.TextColorRed("User not found.\n");
            PressKeyToContinue.Execute();
            return;
        }

        string passwordHashString = reader.GetString(0);
        string saltString = reader.GetString(1);

        byte[] storedPasswordHash = Convert.FromBase64String(passwordHashString);
        byte[] storedPasswordSalt = Convert.FromBase64String(saltString);

        bool isPasswordCorrect = PasswordHasher.VerifyPasswordHash(enteredPassword, storedPasswordHash, storedPasswordSalt);

        if (!isPasswordCorrect)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Invalid password.\n");
            PressKeyToContinue.Execute();
            return;
        }

        reader.Close();

        CommandManagerTransaction.loggedIn = true;

        var getIdSql = """
            SELECT id
            FROM users
            WHERE username = @username
            """;

        command = new NpgsqlCommand(getIdSql, connection);
        command.Parameters.AddWithValue("username", username);
        command.Parameters.AddWithValue("id", id);

        id = (Guid)command.ExecuteScalar();

        Console.Clear();
        ChangeColor.TextColorGreen($"Login successful as {username}.\n");
        PressKeyToContinue.Execute();

        connection.Close();
    }
}
