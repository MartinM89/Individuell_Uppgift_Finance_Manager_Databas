using Individuell_Uppgift.Utilities;
using Npgsql;

namespace Individuell_Uppgift;

class Program
{
    public static bool run = true;

    static void Main(string[] args)
    {
        string connectionString = DatabaseConnection.GetConnectionString();
        using NpgsqlConnection connection = new(connectionString);
        connection.Open();

        IAccountManager accountManager = new PostgresAccountManager(connection);
        // PostgresTransactionManager creates tables, functions and triggers
        ITransactionManager transactionManager = new PostgresTransactionManager(connection);
        // IMenuManager loginMenuManager = new LoginMenuManager();
        IMenuManager userMenuManager = new UserMenuManager();
        userMenuManager.SetMenu(new LoginMenu(connection, accountManager, userMenuManager, transactionManager));

        while (run)
        {
            string? userChoice = string.Empty;
            string? hideUserChoice = HideCursor.Execute(userChoice).ToUpper();
            if (userChoice != null)
            {
                userMenuManager.GetMenu().ExecuteCommand(hideUserChoice.ToUpper());
            }
            else
            {
                break;
            }
        }
    }
}
