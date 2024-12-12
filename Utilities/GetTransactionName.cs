public class GetTransactionName
{
    public static string? Execute()
    {
        while (true)
        {
            Console.Clear();
            Console.Write("Enter name: ");
            string? transactionName = Console.ReadLine();

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
}
