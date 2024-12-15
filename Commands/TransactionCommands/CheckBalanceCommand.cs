using Individuell_Uppgift.Utilities;

public class CheckBalanceCommand : Command
{
    public CheckBalanceCommand(GetManagers getManagers)
        : base('B', "Balance", getManagers) { }

    public override string GetDescription()
    {
        return "Check your current balance";
    }

    public override void Execute()
    {
        decimal totalAmount = GetManagers.TransactionManager.GetBalance();

        while (true)
        {
            Console.Clear();

            Console.Write("Your total balance is ");
            ChangeColor.TextColorGreen($"{totalAmount:N2}");
            Console.WriteLine(".");

            Console.WriteLine("\nSee list of all transactions? [Y/N]");
            string? seeFullListAnswer = HideCursor.Execute().ToUpper();

            if (seeFullListAnswer == null)
            {
                GetManagers.UserMenuManager.ReturnToSameMenu();
                return;
            }

            if (seeFullListAnswer.Equals("N"))
            {
                GetManagers.UserMenuManager.ReturnToSameMenu();
                return;
            }

            if (!seeFullListAnswer.Equals("Y"))
            {
                Console.WriteLine("Invalid Input. [Y/N]");
                continue;
            }

            List<Transaction> transactions = GetManagers.TransactionManager.GetAllTransactions();

            TransactionTable.GetTransactionTableTop();
            TransactionTable.GetMultipleRowsTransactionTableCenter(transactions);
            TransactionTable.GetTransactionsTableBottom();

            PressKeyToContinue.Execute();

            GetManagers.UserMenuManager.ReturnToSameMenu();
            break;
        }
    }
}
