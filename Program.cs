using Npgsql;

namespace Individuell_Uppgift;

class Program
{
    public static bool run = true;

    static void Main(string[] args)
    {
        // var manager = new PostgresTransactionManager(); // Creates tables

        string connectionString = DatabaseConnection.GetConnectionString();

        NpgsqlConnection connection = new(connectionString);
        connection.Open();

        CommandManagerAccount commandManager = new(connection);

        while (run)
        {
            AccountMenu.Execute();
            commandManager.Execute(connection);
        }

        connection.Close();
    }
}
