using Individuell_Uppgift;
using Npgsql;

public class CommandManagerAccount
{
    private readonly NpgsqlConnection Connection;

    public CommandManagerAccount(NpgsqlConnection connection)
    {
        Connection = connection;
    }

    public void Execute(NpgsqlConnection connection)
    {
        UserAccount userAccount = new();

        string userChoice = string.Empty;
        string hideUserChoice = HideCursor.Execute(userChoice).ToUpper();

        switch (hideUserChoice)
        {
            case "C":
                userAccount.Create(connection);
                break;

            case "L":
                userAccount.Login(connection);
                CommandManagerTransaction.Execute(connection);
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
