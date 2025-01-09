using Individuell_Uppgift.Menus;
using Individuell_Uppgift.Utilities;

public class TransactionMenuAdmin : Menu
{
    public TransactionMenuAdmin(GetManagers getManagers)
    {
        AddCommand(new AddTransactionCommand(getManagers));
        AddCommand(new DeleteTransactionCommand(getManagers));
        AddCommand(new CheckBalanceCommand(getManagers));
        AddCommand(new CheckBalancePerUserCommand(getManagers));
        AddCommand(new CheckIncomeCommand(getManagers));
        AddCommand(new CheckExpenseCommand(getManagers));
        AddCommand(new LogoutCommand(getManagers));
        AddCommand(new HelpCommand(getManagers));
    }

    public override void Display()
    {
        Console.Clear();

        Console.WriteLine("Admin Transasction Menu:\n");

        foreach (Command command in commands)
        {
            ChangeColor.TextColorGreen($"[{command.Shortcut}]");
            Console.WriteLine(command.Name[1..]);
        }
    }
}
