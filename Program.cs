using Npgsql;

namespace Individuell_Uppgift;

class Program
{
    public static bool run = true;

    static void Main(string[] args)
    {
        string connectionString = DatabaseConnection.GetConnectionString();

        NpgsqlConnection connection = new(connectionString);
        connection.Open();

        _ = new PostgresTransactionManager(connection); // Creates tables
        CommandManagerAccount commandManagerAccount = new();

        while (run)
        {
            AccountMenu.Execute();
            commandManagerAccount.Execute(connection);
        }

        connection.Close();
    }
}
