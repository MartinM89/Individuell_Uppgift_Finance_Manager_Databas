using Individuell_Uppgift;
using Individuell_Uppgift.Utilities;
using Npgsql;

public class CommandManagerAccount
{
    // private readonly NpgsqlConnection Connection;

    // public CommandManagerAccount(NpgsqlConnection connection)
    // {
    //     Connection = connection;
    // }

    public static async Task Execute(NpgsqlConnection connection)
    {
        CreateAccountCommand createAccount = new(connection);
        LoginCommand loginAccount = new(connection);

        string userChoice = string.Empty;
        string hideUserChoice = HideCursor.Execute(userChoice).ToUpper();

        switch (hideUserChoice)
        {
            case "C":
                await createAccount.Execute();
                break;

            case "L":
                await loginAccount.Execute();
                await CommandManagerTransaction.Execute(connection);
                break;

            case "G":
                // userAccount.GuestLogin();
                Console.Clear();
                Console.WriteLine("Guest Account");
                PressKeyToContinue.Execute();
                break;

            case "E":
                Program.run = false;
                break;

            default:
                Console.WriteLine("Invalid Input.");
                break;
        }
    }
}
