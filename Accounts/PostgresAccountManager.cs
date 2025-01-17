using Individuell_Uppgift.Utilities;
using Npgsql;

public class PostgresAccountManager : IAccountManager
{
    private readonly NpgsqlConnection connection;

    private static Guid loggedInUserId;

    public PostgresAccountManager(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public async Task CreateUser(User user)
    {
        if (user.PasswordHash == null || user.PasswordSalt == null)
        {
            throw new Exception("Password could not be sent to database.");
        }

        string createAccountSql = """
            INSERT INTO users (username, password_hash, password_salt)
            VALUES (@username, @password_hash, @password_salt)
            """;

        try
        {
            NpgsqlCommand command = new(createAccountSql, connection);
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@password_hash", Convert.ToBase64String(user.PasswordHash));
            command.Parameters.AddWithValue("@password_salt", Convert.ToBase64String(user.PasswordSalt));

            await command.ExecuteNonQueryAsync();
        }
        catch (NpgsqlException ex)
        {
            throw new NpgsqlException($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to create an new account to database.");
        }
    }

    public async Task SetLoggedInUserId(string username)
    {
        string getIdSql = """
            SELECT id
            FROM users
            WHERE username = @username
            """;

        try
        {
            NpgsqlCommand getIdCmd = new NpgsqlCommand(getIdSql, connection);
            getIdCmd.Parameters.AddWithValue("@username", username);

            object? result = await getIdCmd.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("User not found!");
            }

            loggedInUserId = (Guid)result;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to set guid from database.");
        }
    }

    public static Guid GetLoggedInUserId()
    {
        return loggedInUserId;
    }

    public static void SetLoggedInUserIdToEmpty()
    {
        loggedInUserId = Guid.Empty;
    }

    public static async Task<bool> CheckLoginDetailsIsCorrect(NpgsqlConnection connection, string username, string enteredPassword)
    {
        string loginSql = """
            SELECT password_hash, password_salt
            FROM users
            WHERE @username = username
            """;

        try
        {
            NpgsqlCommand command = new(loginSql, connection);
            command.Parameters.AddWithValue("@username", username);

            byte[] storedPasswordHash = [];
            byte[] storedPasswordSalt = [];

            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
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
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to verify log in details from database.");
        }
    }

    public async Task<bool> CheckIfUsernameRegistered(string username)
    {
        string checkUsernameSql = """
            SELECT EXISTS (
                SELECT 1
                FROM users
                WHERE username = @username
            )
            """;

        try
        {
            using NpgsqlCommand checkUsernameCmd = new(checkUsernameSql, connection);
            checkUsernameCmd.Parameters.AddWithValue("@username", username);

            object? result = await checkUsernameCmd.ExecuteScalarAsync();
            bool usernameExists = result != null && (bool)result;

            return usernameExists;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to verify if username already exists in database.");
        }
    }

    public async Task<Guid> GetUserGuid(string username)
    {
        string getUserGuidSql = """
            SELECT * FROM users
            WHERE username = @username
            """;

        try
        {
            using NpgsqlCommand getUserGuidCmd = new(getUserGuidSql, connection);
            getUserGuidCmd.Parameters.AddWithValue("@username", username);

            object? result = await getUserGuidCmd.ExecuteScalarAsync();

            if (result != null)
            {
                return (Guid)result;
            }

            return Guid.Empty;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to get guid of logged in user from database.");
        }
    }

    public async Task<bool> GetLoggedInUsername(string username)
    {
        string getUsernameSql = """
            SELECT username FROM users
            WHERE id = @id
            """;

        try
        {
            using NpgsqlCommand getUsernameCmd = new(getUsernameSql, connection);
            getUsernameCmd.Parameters.AddWithValue("@id", loggedInUserId);

            object? result = await getUsernameCmd.ExecuteScalarAsync();

            if (result != null)
            {
                string returnedUsername = (string)result;
                return username.Equals(returnedUsername);
            }

            return false;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to get username of logged in user from database.");
        }
    }

    public async Task CreateUserBonusReward(User user)
    {
        if (user.PasswordHash == null || user.PasswordSalt == null)
        {
            throw new Exception("Password could not be sent to database.");
        }

        using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();

        string createAccountSql = """
            INSERT INTO users (username, password_hash, password_salt)
            VALUES (@username, @password_hash, @password_salt)
            """;

        string gainBonusSql = """
            INSERT INTO transactions (name, amount, user_id) VALUES (@name, @amount, @user_id)
            """;

        try
        {
            NpgsqlCommand accountCommand = new(createAccountSql, connection, transaction);
            accountCommand.Parameters.AddWithValue("@username", user.Username);
            accountCommand.Parameters.AddWithValue("@password_hash", Convert.ToBase64String(user.PasswordHash));
            accountCommand.Parameters.AddWithValue("@password_salt", Convert.ToBase64String(user.PasswordSalt));

            await accountCommand.ExecuteNonQueryAsync();

            Guid userGuid = await GetUserGuid(user.Username);

            NpgsqlCommand transactionCommand = new(gainBonusSql, connection, transaction);
            transactionCommand.Parameters.AddWithValue("@name", "Bonus reward");
            transactionCommand.Parameters.AddWithValue("@amount", 10000);
            transactionCommand.Parameters.AddWithValue("@user_id", userGuid);

            await transactionCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch (NpgsqlException ex)
        {
            await transaction.RollbackAsync();
            throw new NpgsqlException($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to create an new account to database with bonus.");
        }
    }
}
