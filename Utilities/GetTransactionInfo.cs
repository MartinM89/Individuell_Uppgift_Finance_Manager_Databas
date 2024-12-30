using Individuell_Uppgift.Utilities;

public class GetTransactionInfo
{
    public static string? TransactionName()
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

    public static decimal TransactionAmount()
    {
        while (true)
        {
            Console.Clear();
            Console.CursorVisible = true;
            Console.Write("Enter amount: ");
            string? transactionValueString = Console.ReadLine();
            Console.CursorVisible = false;

            if (string.IsNullOrEmpty(transactionValueString))
            {
                return 0;
            }

            transactionValueString = transactionValueString.Replace('.', ',');

            if (!decimal.TryParse(transactionValueString, out decimal transactionValue))
            {
                Console.Clear();
                Console.WriteLine("Invalid Input. Amount can only be numbers.");
                PressKeyToContinue.Execute();
                continue;
            }

            // Subtract 5 to account for spaces and the decimal in the formatted value.
            if (transactionValueString.Length > TransactionTable.amountWidth - 5)
            {
                Console.Clear();
                Console.WriteLine($"Invalid Input. Amount can't exceed {TransactionTable.amountWidth - 5} numbers.");
                PressKeyToContinue.Execute();
                continue;
            }

            if (transactionValue.Equals(0))
            {
                Console.Clear();
                Console.WriteLine("Invalid Input. Amount can't be 0.");
                PressKeyToContinue.Execute();
                continue;
            }

            return transactionValue;
        }
    }

    public static async Task<Guid> TransferTargetUserGuid(GetManagers getManagers)
    {
        Console.Clear();

        Console.Write("Who do you wish to transfer funds to? ");
        string? targetUser = Console.ReadLine();

        if (string.IsNullOrEmpty(targetUser))
        {
            getManagers.UserMenuManager.SetMenu(new TransactionMenu(getManagers));
            return Guid.Empty;
        }

        targetUser = targetUser[..1].ToUpper() + targetUser[1..];

        bool usernameExists = await getManagers.AccountManager.CheckIfUsernameRegistered(targetUser);

        if (!usernameExists)
        {
            Console.Clear();
            ChangeColor.TextColorRed("Invalid Input. Could not find account.\n");
            PressKeyToContinue.Execute();
            getManagers.UserMenuManager.SetMenu(new TransactionMenu(getManagers));
            return Guid.Empty;
        }

        return await getManagers.AccountManager.GetUserGuid(targetUser);
    }
}
