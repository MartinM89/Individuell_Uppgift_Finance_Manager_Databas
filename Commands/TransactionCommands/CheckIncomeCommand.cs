using System.Drawing;
using System.Runtime.InteropServices;
using Npgsql;

public class CheckIncomeCommand : Command
{
    public CheckIncomeCommand()
        : base("Check Income") { }

    public override string GetDescription()
    {
        return "Check your income";
    }

    public override void Execute(NpgsqlConnection connection)
    {
        while (true)
        {
            Console.Clear();

            PostgresTransactionManager getTransaction = new(connection);

            // int transactionCount = TransactionManager.GetTransactionCount();

            // if (transactionCount == 0)
            // {
            //     Console.WriteLine("There are no saved transactions.");
            //     PressKeyToContinue.Execute();
            //     break;
            // }

            Console.WriteLine("Do you wish to see:\n");
            ChangeColor.TextColorGreen("[D]");
            Console.WriteLine("aily");
            ChangeColor.TextColorGreen("[W]");
            Console.WriteLine("eekly");
            ChangeColor.TextColorGreen("[M]");
            Console.WriteLine("onthly");
            ChangeColor.TextColorGreen("[Y]");
            Console.WriteLine("early");

            string userChoice = string.Empty;
            string hideUserChoice = HideCursor.Execute(userChoice).ToUpper();

            if (string.IsNullOrEmpty(hideUserChoice))
            {
                return;
            }

            if (!hideUserChoice.All(Char.IsLetter) && !hideUserChoice.Length.Equals(1))
            {
                Console.Clear();
                Console.WriteLine("Invalid Input. [DWMY]");
                PressKeyToContinue.Execute();
                continue;
            }

            Enum transactionCategory = null!;

            switch (hideUserChoice)
            {
                case "D":
                    transactionCategory = TransactionCategory.Day;
                    break;

                case "W":
                    transactionCategory = TransactionCategory.Week;
                    break;

                case "M":
                    transactionCategory = TransactionCategory.Month;
                    break;

                case "Y":
                    transactionCategory = TransactionCategory.Year;
                    break;

                default:
                    break;
            }

            Console.CursorVisible = true;

            Console.Write($"What {transactionCategory} do you wish to check? ");
            if (int.TryParse(Console.ReadLine()!, out int transactionDate)) { }

            Console.CursorVisible = false;

            char transactionType = '>';

            Console.Clear();

            switch (hideUserChoice)
            {
                case "D":
                    getTransaction.GetTransactionsByDay(transactionDate, transactionType);
                    break;

                case "W":
                    getTransaction.GetTransactionsByWeek(transactionDate, transactionType);
                    break;

                case "M":
                    getTransaction.GetTransactionsByMonth(transactionDate, transactionType);
                    break;

                case "Y":
                    getTransaction.GetTransactionsByYear(transactionDate, transactionType);
                    break;

                default:
                    Console.WriteLine("Invalid Input. [DWMY]");
                    PressKeyToContinue.Execute();
                    continue;
            }

            break;
        }
    }
}
