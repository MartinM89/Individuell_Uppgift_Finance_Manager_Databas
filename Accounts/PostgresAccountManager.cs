using Individuell_Uppgift.Utilities;
using Npgsql;

public class PostgresAccountManager : IAccountManager
{
    private readonly NpgsqlConnection connection;

    public static Guid LoggedInUserId { get; private set; }

    public static bool LoggedIn { get; set; } = false;

    public PostgresAccountManager(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public void Create(User user)
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

    public void GetUserGuid(string username)
    {
        string getIdSql = """
            SELECT id
            FROM users
            WHERE username = @username
            """;

        NpgsqlCommand getIdCmd = new NpgsqlCommand(getIdSql, connection);
        getIdCmd.Parameters.AddWithValue("username", username);

        object? result = getIdCmd.ExecuteScalar();

        if (result == null || result == DBNull.Value)
        {
            throw new InvalidOperationException("User not found!");
        }

        LoggedInUserId = (Guid)result;

        // LoggedInUserId =  getIdCmd.ExecuteScalar() as Guid? ?? throw new InvalidOperationException("User not found!");

        LoggedIn = true;
    }

    public static bool CheckLoginDetailsIsCorrect(NpgsqlConnection connection, string username, string enteredPassword)
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
        if (reader.Read())
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

    public bool CheckUsernameRegistered(string username)
    {
        var checkUsernameSql = """
            SELECT EXISTS (
                SELECT 1
                FROM users
                WHERE username = @username
            )
            """;

        var command = new NpgsqlCommand(checkUsernameSql, connection);
        command.Parameters.AddWithValue("username", username);

        object? result = command.ExecuteScalar();
        bool usernameExists = result != null && (bool)result;

        return usernameExists;
    }
}
