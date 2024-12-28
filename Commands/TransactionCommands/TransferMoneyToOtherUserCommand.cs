using Individuell_Uppgift.Utilities;

public class TransferMoneyToOtherUserCommand : Command
{
    public TransferMoneyToOtherUserCommand(GetManagers getManagers)
        : base('T', "Transfer", getManagers) { }

    public override string GetDescription()
    {
        return "Transfer money to another user";
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

        if (amount < 0)
        {
            Console.Clear();
            ChangeColor.TextColorRed($"You can't send expenses to other users.\n");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.SetMenu(new TransactionMenu(GetManagers));
            return;
        }

        decimal balance = GetManagers.TransactionManager.GetBalance(PostgresAccountManager.GetLoggedInUserId());

        if (balance < amount)
        {
            Console.Clear();
            ChangeColor.TextColorRed($"You do not have enough money to send {amount}:-.\n");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.SetMenu(new TransactionMenu(GetManagers));
            return;
        }

        Console.Write("Who do you wish to send money to? "); // Method out of this
        string username = Console.ReadLine()!;

        username = username[..1].ToUpper() + username[1..];

        bool usernameExists = GetManagers.AccountManager.CheckIfUsernameRegistered(username);

        if (!usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Could not find account.\n");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.SetMenu(new TransactionMenu(GetManagers));
            return;
        }

        Guid sendToGuid = GetManagers.AccountManager.GetUserGuid(username);

        Transaction transaction = new(1, name, amount, DateTime.Now, sendToGuid);

        GetManagers.TransactionManager.TransferFunds(transaction);

        Console.Clear();
        Console.WriteLine($"The following transaction has been send to {name}:");
        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetSingleRowTransactionTableCenter(transaction);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
