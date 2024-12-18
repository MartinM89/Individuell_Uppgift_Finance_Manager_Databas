public class AddTransactionCommand : Command
{
    public AddTransactionCommand(GetManagers getManagers)
        : base('A', "Add", getManagers) { }

    public override string GetDescription()
    {
        return "Adds a transaction";
    }

    public override void Execute()
    {
        Console.Clear();

        string? name = GetTransactionName.Execute();

        if (string.IsNullOrEmpty(name))
        {
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        decimal amount = GetTransactionAmount.Execute();

        if (amount.Equals(0))
        {
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        Guid userId = Guid.Empty;
        if (GetManagers.AccountManager.GetLoggedInUsername("Admin"))
        {
            Console.Write("Username: ");
            string? targetUser = Console.ReadLine();

            if (string.IsNullOrEmpty(targetUser))
            {
                Console.WriteLine("Invalid username."); // Fix
                PressKeyToContinue.Execute();
                GetManagers.UserMenuManager.ReturnToSameMenu();
                return;
            }

            userId = GetManagers.AccountManager.GetUserGuid(targetUser);
            if (userId == Guid.Empty)
            {
                Console.WriteLine("User not found."); // Fix
                GetManagers.UserMenuManager.ReturnToSameMenu();
                PressKeyToContinue.Execute();
                return;
            }
        }

        Transaction transaction;
        if (userId != Guid.Empty)
        {
            transaction = new(1, name, amount, DateTime.Now, userId);
        }
        else
        {
            transaction = new(1, name, amount, DateTime.Now, PostgresAccountManager.GetLoggedInUserId());
        }

        GetManagers.TransactionManager.AddTransaction(transaction);

        Console.Clear();
        Console.WriteLine("The following transaction has been added:");
        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetSingleRowTransactionTableCenter(transaction);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
