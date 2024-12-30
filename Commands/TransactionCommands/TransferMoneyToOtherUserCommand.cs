using Individuell_Uppgift.Utilities;

public class TransferMoneyToOtherUserCommand : Command
{
    public TransferMoneyToOtherUserCommand(GetManagers getManagers)
        : base('T', "Transfer", getManagers) { }

    public override string GetDescription()
    {
        return "Transfer money to another user";
    }

    public override async Task Execute()
    {
        Console.Clear();

        string? name = GetTransactionInfo.TransactionName();

        if (string.IsNullOrEmpty(name))
        {
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        decimal amount = GetTransactionInfo.TransactionAmount();

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

        decimal balance = await GetManagers.TransactionManager.GetBalance(PostgresAccountManager.GetLoggedInUserId());

        if (balance < amount)
        {
            Console.Clear();
            ChangeColor.TextColorRed($"You do not have enough money to send {amount}:-.\n");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.SetMenu(new TransactionMenu(GetManagers));
            return;
        }

        Guid sendToGuid = await GetTransactionInfo.TransferTargetUserGuid(GetManagers);

        if (sendToGuid.Equals(Guid.Empty))
        {
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        Transaction transaction = new(1, name, amount, DateTime.Now, sendToGuid);

        await GetManagers.TransactionManager.TransferFunds(transaction);

        Console.Clear();
        Console.WriteLine($"The following transaction has been send to {name}:");
        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetSingleRowTransactionTableCenter(transaction);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
