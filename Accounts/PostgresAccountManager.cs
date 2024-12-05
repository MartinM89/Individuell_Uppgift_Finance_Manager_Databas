using Npgsql;

public class PostgresAccountManager : IAccount
{
    public static Guid LoggedInUserId { get; private set; }

    public static bool loggedIn = false;

    public void Create(NpgsqlConnection connection, User user)
    {
        string createAccountSql = """
            INSERT INTO users (username, password_hash, password_salt)
            VALUES (@username, @password_hash, @password_salt)
            """;

        NpgsqlCommand command = new(createAccountSql, connection);
        command.Parameters.AddWithValue("username", user.Username);
        command.Parameters.AddWithValue("password_hash", Convert.ToBase64String(user.PasswordHash!));
        command.Parameters.AddWithValue("password_salt", Convert.ToBase64String(user.PasswordSalt!));

        command.ExecuteNonQuery();
    }

    public void Login(NpgsqlConnection connection, string username)
    {
        string getIdSql = """
            SELECT id
            FROM users
            WHERE username = @username
            """;

        NpgsqlCommand command = new NpgsqlCommand(getIdSql, connection);
        command.Parameters.AddWithValue("username", username);

        LoggedInUserId = (Guid)command.ExecuteScalar()!;

        loggedIn = true;
    }

    public static bool CheckLoginDetailsIsCorrect(NpgsqlConnection connection, string username, string enteredPassword) // Should I include a try-catch here. For example if database is down due to connection issues.
    {
        string loginSql = """
            SELECT password_hash, password_salt
            FROM users
            WHERE username = @username
            """;

        NpgsqlCommand command = new(loginSql, connection);
        command.Parameters.AddWithValue("username", username);

        byte[] storedPasswordHash = [];
        byte[] storedPasswordSalt = [];

        using NpgsqlDataReader reader = command.ExecuteReader();
        if (reader.Read()) // Confused
        {
            string passwordHashString = reader.GetString(0);
            string saltString = reader.GetString(1);

            storedPasswordHash = Convert.FromBase64String(passwordHashString);
            storedPasswordSalt = Convert.FromBase64String(saltString);
        }

        bool isPasswordCorrect = PasswordHasher.VerifyPasswordHash(enteredPassword, storedPasswordHash, storedPasswordSalt);

        return isPasswordCorrect;
    }

    public static Guid GetLoggedInUserId()
    {
        return LoggedInUserId;
    }
}
