using Individuell_Uppgift.Utilities;

public class GetTransactionName
{
    public static string? Execute()
    {
        while (true)
        {
            Console.Clear();
            Console.CursorVisible = true;
            Console.Write("Enter name: ");
            string? transactionName = Console.ReadLine();
            Console.CursorVisible = false;

            if (string.IsNullOrEmpty(transactionName))
            {
                return null;
            }

            bool onlyLettersOrWhiteSpace = transactionName.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));

            if (!onlyLettersOrWhiteSpace)
            {
                Console.Clear();
                Console.WriteLine("Invalid Input. Name can't contain numbers or symbols.");
                PressKeyToContinue.Execute();
                continue;
            }

            if (transactionName.Length! < 3 || transactionName.Length > TransactionTable.nameWidth)
            {
                Console.Clear();
                Console.WriteLine($"Invalid Input. Name must be between 3 - {TransactionTable.nameWidth} characters long.");
                PressKeyToContinue.Execute();
                continue;
            }

            return char.ToUpper(transactionName[0]) + transactionName[1..];
        }
    }

    public static Guid GetUserGuid(GetManagers getManagers)
    {
        Console.Write("Who do you wish to send money to? ");
        string username = Console.ReadLine()!;

        username = username[..1].ToUpper() + username[1..];

        bool usernameExists = getManagers.AccountManager.CheckIfUsernameRegistered(username);

        if (!usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Could not find account.\n");
            PressKeyToContinue.Execute();
            getManagers.UserMenuManager.SetMenu(new TransactionMenu(getManagers));
            return Guid.Empty;
        }

        return getManagers.AccountManager.GetUserGuid(username);
    }
}
