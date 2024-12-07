using Individuell_Uppgift.Utilities;
using Npgsql;

public class PostgresAccountManager : IAccountManager
{
    private NpgsqlConnection connection; // Readonly?

    public static Guid LoggedInUserId { get; private set; }

    public static bool LoggedIn { get; set; } = false;

    public PostgresAccountManager(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public async Task Create(NpgsqlConnection connection, User user)
    {
        string createAccountSql = """
            INSERT INTO users (username, password_hash, password_salt)
            VALUES (@username, @password_hash, @password_salt)
            """;

        NpgsqlCommand command = new(createAccountSql, connection);
        command.Parameters.AddWithValue("username", user.Username);
        command.Parameters.AddWithValue("password_hash", Convert.ToBase64String(user.PasswordHash!));
        command.Parameters.AddWithValue("password_salt", Convert.ToBase64String(user.PasswordSalt!));

        await command.ExecuteNonQueryAsync();
    }

    public async Task Login(NpgsqlConnection connection, string username)
    {
        string getIdSql = """
            SELECT id
            FROM users
            WHERE username = @username
            """;

        NpgsqlCommand getIdCmd = new NpgsqlCommand(getIdSql, connection);
        getIdCmd.Parameters.AddWithValue("username", username);

        // object? result = await getIdCmd.ExecuteScalarAsync();

        // if (result == null || result == DBNull.Value)
        // {
        //     throw new InvalidOperationException("User not found!");
        // }

        // LoggedInUserId = (Guid)result;

        LoggedInUserId = await getIdCmd.ExecuteScalarAsync() as Guid? ?? throw new InvalidOperationException("User not found!");

        LoggedIn = true;
    }

    public static async Task<bool> CheckLoginDetailsIsCorrect(NpgsqlConnection connection, string username, string enteredPassword)
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

        using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
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
}
