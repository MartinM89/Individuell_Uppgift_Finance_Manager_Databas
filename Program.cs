using System.Transactions;

namespace Individuell_Uppgift;

class Program
{
    public static bool run = true;

    static void Main(string[] args)
    {
        // LoadCommand.Execute();
        // RunFirstTime.Execute();

        var manager = new PostgresTransactionManager();
        var createAccountCommand = new CreateAccountCommand();
        var loginCommand = new LoginCommand();

        while (run)
        {
            Console.Clear();

            Console.WriteLine("[C]reate");
            Console.WriteLine("[L]ogin");

            Console.Write("\nEnter Choice: ");

            string userChoice = Console.ReadLine()!;

            CommandCenter.ExecuteLogin(userChoice);
        }
    }
}
