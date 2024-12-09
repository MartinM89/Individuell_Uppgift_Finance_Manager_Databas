using Individuell_Uppgift;
using Individuell_Uppgift.Utilities;
using Npgsql;

public class CommandManagerAccount
{
    public static async Task Execute(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
    {
        CreateAccountCommand createAccount = new(connection, accountManager, menuManager, transactionManager);
        LoginCommand loginAccount = new(connection, accountManager, menuManager, transactionManager);

        string userChoice = string.Empty;
        string hideUserChoice = HideCursor.Execute(userChoice).ToUpper();

        switch (hideUserChoice)
        {
            case "C":
                await createAccount.Execute();
                break;

            case "L":
                await loginAccount.Execute();
                await CommandManagerTransaction.Execute(connection, accountManager, menuManager, transactionManager);
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
