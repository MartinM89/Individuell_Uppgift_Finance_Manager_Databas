namespace Individuell_Uppgift;

class Program
{
    public static bool run = true;

    static void Main(string[] args)
    {
        // var manager = new PostgresTransactionManager(); // Creates tables

        while (run)
        {
            AccountMenu.Execute();
            CommandManagerAccount.Execute();
        }
    }
}
