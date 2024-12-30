using Individuell_Uppgift.Utilities;
using Npgsql;

namespace Individuell_Uppgift;

class Program
{
    public static bool run = true;

    static async Task Main(string[] args)
    {
        Console.CursorVisible = false;
        string connectionString = Database.GetConnectionString();

        NpgsqlConnection? connection;
        try
        {
            connection = new(connectionString);
            await connection.OpenAsync();
        }
        catch (NpgsqlException ex)
        {
            throw new NpgsqlException($"Can't access database {ex.Message}");
        }

        try
        {
            await Database.Initialize(connection);

            ITransactionManager transactionManager = new PostgresTransactionManager(connection);
            IAccountManager accountManager = new PostgresAccountManager(connection);
            IMenuManager userMenuManager = new UserMenuManager();
            GetManagers getManagers = new(connection, accountManager, transactionManager, userMenuManager);
            userMenuManager.SetMenu(new LoginMenu(getManagers));

            while (run)
            {
                string? userChoice = HideCursor.Input().ToUpper();

                if (userChoice != null)
                {
                    await userMenuManager.GetMenu().ExecuteCommand(userChoice.ToUpper());
                }
                else
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
    }
}
