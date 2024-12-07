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

        IAccountManager accountManager = new PostgresAccountManager(connection);
        // PostgresTransactionManager creates tables, functions and triggers
        ITransactionManager transactionManager = new PostgresTransactionManager(connection);
        // IMenuManager loginMenuManager = new LoginMenuManager();
        IMenuManager userMenuManager = new UserMenuManager();
        Menu loginMenu = new LoginMenu(connection);
        userMenuManager.SetMenu(loginMenu);

        // Creates tables, functions and triggers
        // _ = new PostgresTransactionManager(connection); // Required?

        while (run)
        {
            AccountMenu.Execute();
            await CommandManagerAccount.Execute(connection);
            // string? userChoice = string.Empty;
            // string? hideUserChoice = HideCursor.Execute(userChoice).ToUpper();
            // if (userChoice != null)
            // {
            //     userMenuManager.GetMenu().ExecuteCommand(hideUserChoice.ToUpper());
            // }
            // else
            // {
            //     break;
            // }
        }
    }
}


// IAccountManager
// IMenuManager
// ITransactionManager
