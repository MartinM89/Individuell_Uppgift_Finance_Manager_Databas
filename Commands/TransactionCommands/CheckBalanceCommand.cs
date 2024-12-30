using Individuell_Uppgift.Utilities;

public class CheckBalanceCommand : Command
{
    public CheckBalanceCommand(GetManagers getManagers)
        : base('B', "Balance", getManagers) { }

    public override string GetDescription()
    {
        return "Check your current balance";
    }

    public override async Task Execute()
    {
        var (userGuid, targetUser, adminLoggedIn) = await GetGuidForAdmin.Execute(GetManagers);

        while (true)
        {
            Console.Clear();

            decimal totalAmount = await GetManagers.TransactionManager.GetBalance(userGuid);

            string balanceMessage = targetUser.Equals(string.Empty) ? "Your total balance is " : $"{targetUser}'s balance is ";

            Console.Write(balanceMessage);
            ChangeColor.TextColorGreen($"{totalAmount:N2}");
            Console.WriteLine(".");

            Console.WriteLine("\nSee list of all transactions? [Y/N]");
            string? seeFullListAnswer = HideCursor.Input().ToUpper();

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

            Console.Clear();

            List<Transaction> transactions = await GetManagers.TransactionManager.GetAllTransactions(userGuid);

            TransactionTable.GetTransactionTableTop();
            TransactionTable.GetMultipleRowsTransactionTableCenter(transactions);
            TransactionTable.GetTransactionsTableBottom();

            PressKeyToContinue.Execute();

            GetManagers.UserMenuManager.ReturnToSameMenu();
            break;
        }
    }
}
