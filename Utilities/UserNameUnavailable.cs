using Npgsql;

namespace Individuell_Uppgift.Utilities;

public class UserNameUnavailable
{
    public static bool Execute(NpgsqlConnection connection, string username)
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


// Useless?
