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
            throw new NpgsqlException($"PostgreSQL Error: {ex.Message}\nCouldn't access database.", ex);
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
                // string? userChoice = HideCursor.Input().ToUpper();
                string? userChoice = HideCursor.Input().ToUpper();
                _ = char.TryParse(userChoice, out char userChoiceChar);

                if (userChoice != null)
                {
                    await userMenuManager.GetMenu().ExecuteCommand(userChoiceChar);
                }
                else
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}\nAn error occured.", ex);
        }
        finally
        {
            connection.Close();
        }
    }
}
