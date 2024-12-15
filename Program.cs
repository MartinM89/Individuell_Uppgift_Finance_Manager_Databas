using Individuell_Uppgift.Utilities;
using Npgsql;

namespace Individuell_Uppgift;

class Program
{
    public static bool run = true;

    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        string connectionString = Database.GetConnectionString();

        NpgsqlConnection? connection;
        try
        {
            connection = new(connectionString);
            connection.Open();
        }
        catch (NpgsqlException ex)
        {
            throw new NpgsqlException($"Can't access database {ex.Message}");
        }

        Database.Initialize(connection);

        ITransactionManager transactionManager = new PostgresTransactionManager(connection);
        IAccountManager accountManager = new PostgresAccountManager(connection);
        IMenuManager userMenuManager = new UserMenuManager();
        GetManagers getManagers = new(connection, accountManager, transactionManager, userMenuManager);
        userMenuManager.SetMenu(new LoginMenu(getManagers));

        while (run)
        {
            string? userChoice = HideCursor.Execute().ToUpper();
            if (userChoice != null)
            {
                userMenuManager.GetMenu().ExecuteCommand(userChoice.ToUpper());
            }
            else
            {
                break;
            }
        }
    }
}
