namespace Individuell_Uppgift;

class Program
{
    public static bool run = true;

    static void Main(string[] args)
    {
        // LoadCommand.Execute();
        // RunFirstTime.Execute();

        // var manager = new PostgresTransactionManager();
        // var createAccountCommand = new CreateAccountCommand();
        // var loginCommand = new LoginCommand();

        while (run)
        {
            AccountMenu.Execute();
            CommandManagerAccount.Execute();
        }
    }
}
