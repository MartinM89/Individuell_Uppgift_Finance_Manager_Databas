using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;

public class TransactionMenu : Menu
{
    public TransactionMenu(GetManagers getManagers)
    {
        AddCommand(new AddTransactionCommand(getManagers));
        AddCommand(new DeleteTransactionCommand(getManagers));
        AddCommand(new CheckBalanceCommand(getManagers));
        AddCommand(new TransferMoneyToOtherUserCommand(getManagers));
        AddCommand(new CheckIncomeCommand(getManagers));
        AddCommand(new CheckExpenseCommand(getManagers));
        AddCommand(new CheckAllTransactionsCommand(getManagers));
        AddCommand(new LogoutCommand(getManagers));
        AddCommand(new HelpCommand(getManagers));
    }

    public override void Display()
    {
        Console.Clear();

        Console.WriteLine($"Transasction Menu:\n");

        foreach (Command command in commands)
        {
            ChangeColor.TextColorGreen($"[{command.Shortcut}]");
            Console.WriteLine(command.Name[1..]);
        }
    }
}
