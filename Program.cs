using Individuell_Uppgift.Utilities;
using Npgsql;

namespace Individuell_Uppgift;

class Program
{
    public static bool run = true;

    static void Main(string[] args)
    {
        string connectionString = DatabaseConnection.GetConnectionString();

        NpgsqlConnection? connection = null;

        try
        {
            connection = new(connectionString);
            connection.Open();
        }
        catch (NpgsqlException ex)
        {
            throw new NpgsqlException($"Can't access database {ex.Message}");
        }

        // 'PostgresTransactionManager' creates tables, functions and triggers
        ITransactionManager transactionManager = new PostgresTransactionManager(connection);
        IAccountManager accountManager = new PostgresAccountManager(connection);
        IMenuManager userMenuManager = new UserMenuManager();
        GetManagers getManagers = new(connection, accountManager, transactionManager, userMenuManager);
        userMenuManager.SetMenu(new LoginMenu(getManagers));

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
