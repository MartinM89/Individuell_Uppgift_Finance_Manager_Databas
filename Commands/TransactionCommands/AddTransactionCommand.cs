using Individuell_Uppgift.Utilities;

public class AddTransactionCommand : Command
{
    public AddTransactionCommand(GetManagers getManagers)
        : base('A', "Add", getManagers) { }

    public override string GetDescription()
    {
        return "Adds a transaction";
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

        var (userGuid, targetUser, adminLoggedIn) = await GetGuidForAdmin.Execute(GetManagers);

        if (targetUser.Equals(string.Empty))
        {
            targetUser = "your account";
        }

        Transaction transaction;
        if (userGuid.Equals(Guid.Empty))
        {
            transaction = new(1, name, amount, DateTime.Now, PostgresAccountManager.GetLoggedInUserId());
        }
        else
        {
            transaction = new(1, name, amount, DateTime.Now, userGuid);
        }

        if (adminLoggedIn && userGuid.Equals(Guid.Empty))
        {
            Console.Clear();
            ChangeColor.TextColorRed("Could not find account.");
            PressKeyToContinue.Execute();
            GetManagers.UserMenuManager.ReturnToSameMenu();
            return;
        }

        await GetManagers.TransactionManager.AddTransaction(transaction);

        Console.Clear();
        Console.WriteLine($"The following transaction has been added to {targetUser}.");
        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetSingleRowTransactionTableCenter(transaction);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();
        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
