using Npgsql;

public class UserNameUnavailable
{
    public static bool Execute(string username, NpgsqlConnection connection)
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

        // bool usernameExists = (bool)command.ExecuteScalar(); // Same as above

        return usernameExists;
    }
}
