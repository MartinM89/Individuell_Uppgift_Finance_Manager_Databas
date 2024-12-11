public class GetTransactionName
{
    public static string Execute()
    {
        string capitalizedTransactionName;

        while (true)
        {
            Console.Clear();
            Console.Write("Enter name: ");
            string? transactionName = Console.ReadLine();
            // csharpier-ignore
            if (string.IsNullOrEmpty(transactionName)) { return null!; }

            bool onlyLettersOrWhiteSpace = true;

            foreach (char c in transactionName)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    onlyLettersOrWhiteSpace = false;
                    break;
                }
            }

            // bool onlyLettersOrWhiteSpace = transactionName.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)); // Worse than foreach loop

            if (!onlyLettersOrWhiteSpace)
            {
                Console.WriteLine("Invalid Input. Name can't contain numbers or symbols.");
                PressKeyToContinue.Execute();
                continue;
            }

            if (transactionName.Length! < 3 || transactionName.Length > TransactionTable.nameWidth)
            {
                Console.Clear();
                Console.WriteLine("Invalid Input. Name must be between 3 - 21 characters long.");
                PressKeyToContinue.Execute();
                continue;
            }

            return capitalizedTransactionName = char.ToUpper(transactionName[0]) + transactionName[1..];
        }
    }
}
