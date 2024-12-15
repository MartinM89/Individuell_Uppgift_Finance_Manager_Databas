using Individuell_Uppgift.Utilities;
using Npgsql;

public class PostgresAccountManager : IAccountManager
{
    private readonly NpgsqlConnection connection;

    public static Guid LoggedInUserId { get; private set; }

    public PostgresAccountManager(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public void CreateUser(User user)
    {
        string createAccountSql = """
            INSERT INTO users (username, password_hash, password_salt)
            VALUES (@username, @password_hash, @password_salt)
            """;

        NpgsqlCommand command = new(createAccountSql, connection);
        command.Parameters.AddWithValue("username", user.Username);
        command.Parameters.AddWithValue("password_hash", Convert.ToBase64String(user.PasswordHash));
        command.Parameters.AddWithValue("password_salt", Convert.ToBase64String(user.PasswordSalt));

        command.ExecuteNonQuery();
    }

    public void SetLoggedInUserId(string username)
    {
        string getIdSql = """
            SELECT id
            FROM users
            WHERE username = @username
            """;

        try
        {
            NpgsqlCommand getIdCmd = new NpgsqlCommand(getIdSql, connection);
            getIdCmd.Parameters.AddWithValue("username", username);

            object? result = getIdCmd.ExecuteScalar();

            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("User not found!");
            }

            LoggedInUserId = (Guid)result;

            // LoggedInUserId =  getIdCmd.ExecuteScalar() as Guid? ?? throw new InvalidOperationException("User not found!");
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to set guid from database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to set guid.", ex);
        }
    }

    public static Guid GetLoggedInUserId()
    {
        return LoggedInUserId;
    }

    public static bool CheckLoginDetailsIsCorrect(NpgsqlConnection connection, string username, string enteredPassword)
    {
        string loginSql = """
            SELECT password_hash, password_salt
            FROM users
            WHERE username = @username
            """;

        try
        {
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
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to verify log in details from database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to verify log in details.", ex);
        }
    }

    public bool CheckIfUsernameRegistered(string username)
    {
        var checkUsernameSql = """
            SELECT EXISTS (
                SELECT 1
                FROM users
                WHERE username = @username
            )
            """;

        try
        {
            var command = new NpgsqlCommand(checkUsernameSql, connection);
            command.Parameters.AddWithValue("username", username);

            object? result = command.ExecuteScalar();
            bool usernameExists = result != null && (bool)result;

            return usernameExists;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to verify if username already exists in database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to verify if username already exists.", ex);
        }
    }
}
