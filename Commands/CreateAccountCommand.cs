using Npgsql;

public class CreateAccountCommand : Command
{
    private NpgsqlConnection connection;

    public CreateAccountCommand()
        : base("Create Account") { }

    public override string GetDescription()
    {
        return "Create a new account";
    }

    public override void RunCommand()
    {
        Console.Clear();

        string password = string.Empty;
        string confirmPassword = string.Empty;

        Console.Write("Enter username: ");
        string username = Console.ReadLine()!;

        Console.Write("Enter password: ");
        password = HidePassword.Execute(password);
        Console.Write("\nRetype password: ");
        confirmPassword = HidePassword.Execute(confirmPassword);

        if (!password.Equals(confirmPassword))
        {
            Console.WriteLine("Passwords do not match");
            PressKeyToContinue.Execute();
            return;
        }

        PasswordHasher.CreatePasswordHash(
            password,
            out byte[] passwordHash,
            out byte[] passwordSalt
        );

        string connectionString = DatabaseConnection.GetConnectionString();

        this.connection = new NpgsqlConnection(connectionString);
        connection.Open();

        var createAccountSql = """
            INSERT INTO users (username, password_hash, password_salt)
            VALUES (@username, @password_hash, @password_salt)
            """;

        var command = new NpgsqlCommand(createAccountSql, connection);
        command.Parameters.AddWithValue("username", username);
        command.Parameters.AddWithValue("password_hash", Convert.ToBase64String(passwordHash));
        command.Parameters.AddWithValue("password_salt", Convert.ToBase64String(passwordSalt));

        command.ExecuteNonQuery();

        Console.WriteLine("Registered successfully.");

        connection.Close();
    }
}
