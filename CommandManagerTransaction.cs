public class CommandManagerTransaction
{
    public static bool loggedIn = true;

    public static void Execute()
    {
        while (loggedIn)
        {
            TransactionMenu.Execute();

            string userChoice = string.Empty;
            string hideUserChoice = HideCursor.Execute(userChoice).ToUpper();

            switch (hideUserChoice)
            {
                case "A"
                or "ADD":
                    Console.Clear();
                    Console.WriteLine("Add transaction");
                    PressKeyToContinue.Execute();
                    break;

                case "D"
                or "DELETE":
                    Console.Clear();
                    Console.WriteLine("Delete transaction");
                    PressKeyToContinue.Execute();
                    break;

                case "B"
                or "BALANCE":
                    Console.Clear();
                    Console.WriteLine("Balance");
                    PressKeyToContinue.Execute();
                    break;

                case "I"
                or "INCOME":
                    Console.Clear();
                    Console.WriteLine("Income Summary");
                    PressKeyToContinue.Execute();
                    break;

                case "E"
                or "EXPENSE":
                    Console.Clear();
                    Console.WriteLine("Expense Summary");
                    PressKeyToContinue.Execute();
                    break;

                case "L"
                or "Log Out":
                    loggedIn = false;
                    Console.Clear();
                    Console.WriteLine("Thank you for using your personal finance app.");
                    PressKeyToContinue.Execute();
                    break;

                case "H"
                or "HELP":
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
