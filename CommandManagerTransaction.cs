public class CommandManagerTransaction
{
    public static bool loggedIn = false;

    public static void Execute()
    {
        while (loggedIn)
        {
            TransactionMenu.Execute();

            string userChoice = string.Empty;
            string hideUserChoice = HideCursor.Execute(userChoice).ToUpper();

            switch (hideUserChoice)
            {
                case "A":
                    var addTransaction = new AddTransactionCommand();
                    addTransaction.RunCommand();
                    break;

                case "D":
                    Console.Clear();
                    Console.WriteLine("Delete transaction");
                    PressKeyToContinue.Execute();
                    break;

                case "B":
                    Console.Clear();
                    Console.WriteLine("Balance");
                    PressKeyToContinue.Execute();
                    break;

                case "I":
                    Console.Clear();
                    Console.WriteLine("Income Summary");
                    PressKeyToContinue.Execute();
                    break;

                case "E":
                    Console.Clear();
                    Console.WriteLine("Expense Summary");
                    PressKeyToContinue.Execute();
                    break;

                case "L":
                    loggedIn = false;
                    Console.Clear();
                    Console.WriteLine("Thank you for using your personal finance app.");
                    PressKeyToContinue.Execute();
                    break;

                case "H":
                    Console.Clear();
                    Console.WriteLine("Help menu");
                    PressKeyToContinue.Execute();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Invalid Input.");
                    PressKeyToContinue.Execute();
                    break;
            }
        }
    }
}
