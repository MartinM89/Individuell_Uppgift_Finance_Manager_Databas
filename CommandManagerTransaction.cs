using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;
using Npgsql;

public class CommandManagerTransaction
{
    public static async Task Execute(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
    {
        while (PostgresAccountManager.LoggedIn)
        {
            TransactionMenu.Execute();

            string userChoice = string.Empty;
            string hideUserChoice = HideCursor.Execute(userChoice).ToUpper();

            switch (hideUserChoice)
            {
                case "A":
                    AddTransactionCommand addTransaction = new(connection, accountManager, menuManager, transactionManager);
                    await addTransaction.Execute();
                    break;

                case "D":
                    Console.Clear();
                    DeleteTransactionCommand deleteTransaction = new(connection, accountManager, menuManager, transactionManager);
                    await deleteTransaction.Execute();
                    break;

                case "B":
                    Console.Clear();
                    CheckBalanceCommand checkBalance = new(connection, accountManager, menuManager, transactionManager);
                    await checkBalance.Execute();
                    break;

                case "I":
                    Console.Clear();
                    CheckIncomeCommand checkIncome = new(connection, accountManager, menuManager, transactionManager);
                    await checkIncome.Execute();
                    // PressKeyToContinue.Execute();
                    break;

                case "E":
                    Console.Clear();
                    Console.WriteLine("Expense Summary");
                    PressKeyToContinue.Execute();
                    break;

                case "L":
                    PostgresAccountManager.LoggedIn = false;
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
                    CheckAllTransactionsCommand checkAllTransactions = new(connection, accountManager, menuManager, transactionManager);
                    await checkAllTransactions.Execute();
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
