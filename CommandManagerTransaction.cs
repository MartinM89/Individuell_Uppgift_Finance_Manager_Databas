using Npgsql;

public class CommandManagerTransaction
{
    public static void Execute(NpgsqlConnection connection)
    {
        while (PostgresAccountManager.loggedIn)
        {
            TransactionMenu.Execute();

            string userChoice = string.Empty;
            string hideUserChoice = HideCursor.Execute(userChoice).ToUpper();

            switch (hideUserChoice)
            {
                case "A":
                    AddTransactionCommand addTransaction = new();
                    addTransaction.Execute(connection);
                    break;

                case "D":
                    Console.Clear();
                    DeleteTransactionCommand deleteTransaction = new();
                    deleteTransaction.Execute(connection);
                    break;

                case "B":
                    Console.Clear();
                    PostgresTransactionManager getBalance = new(connection);
                    getBalance.GetBalance();
                    break;

                case "I":
                    Console.Clear();
                    CheckIncomeCommand checkIncome = new();
                    checkIncome.Execute(connection);
                    PressKeyToContinue.Execute();
                    break;

                case "E":
                    Console.Clear();
                    Console.WriteLine("Expense Summary");
                    PressKeyToContinue.Execute();
                    break;

                case "L":
                    PostgresAccountManager.loggedIn = false;
                    Console.Clear();
                    Console.WriteLine("Thank you for using your personal finance app.");
                    PressKeyToContinue.Execute();
                    break;

                case "H":
                    Console.Clear();
                    Console.WriteLine("Help menu");
                    PressKeyToContinue.Execute();
                    break;

                case "P":
                    Console.Clear();
                    PostgresTransactionManager getAllTransactions = new(connection);
                    getAllTransactions.GetAllTransactions();
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
