using System.Data;
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

        int connectionAttempts = 1;

        NpgsqlConnection? connection = null;

        while (connectionAttempts <= 3)
        {
            connection = new(connectionString);

            try
            {
                await connection.OpenAsync();
            }
            catch (NpgsqlException)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    for (int i = 3; i > 0; i--)
                    {
                        Console.Clear();
                        Console.WriteLine($"Attempt: {connectionAttempts}. Couldn't access database.");
                        Console.WriteLine($"Retrying in {i} seconds...");
                        Thread.Sleep(1000);

                        if (i == 1)
                        {
                            Console.Clear();
                            Console.WriteLine("Retrying now...");
                            Thread.Sleep(1000);
                        }
                    }

                    connectionAttempts++;
                    continue;
                }
            }

            break;
        }

        if (connection == null)
        {
            throw new Exception("Couldn't access database.");
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
            Console.Clear();
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
