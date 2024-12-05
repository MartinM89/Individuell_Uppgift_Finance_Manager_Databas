using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;
using Npgsql;

namespace Individuell_Uppgift;

class Program
{
    public static bool run = true;

    static async Task Main(string[] args)
    {
        string connectionString = DatabaseConnection.GetConnectionString();
        using NpgsqlConnection connection = new(connectionString);
        connection.Open();

        // Creates tables, functions and triggers
        _ = new PostgresTransactionManager(connection);

        while (run)
        {
            AccountMenu.Execute();
            await CommandManagerAccount.Execute(connection);
        }
    }
}
