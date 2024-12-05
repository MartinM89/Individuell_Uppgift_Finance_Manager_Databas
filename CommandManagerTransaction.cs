using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;
using Npgsql;

public class CommandManagerTransaction
{
    public static async Task Execute(NpgsqlConnection connection)
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
                    await addTransaction.Execute(connection);
                    break;

                case "D":
                    Console.Clear();
                    DeleteTransactionCommand deleteTransaction = new();
                    await deleteTransaction.Execute(connection);
                    break;

                case "B":
                    Console.Clear();
                    CheckBalanceCommand checkBalance = new();
                    await checkBalance.Execute(connection);
                    break;

                case "I":
                    Console.Clear();
                    CheckIncomeCommand checkIncome = new();
                    await checkIncome.Execute(connection);
                    // PressKeyToContinue.Execute();
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
                    CheckAllTransactionsCommand checkAllTransactions = new();
                    await checkAllTransactions.Execute(connection);
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
