public class GetTransactionAmount
{
    public static decimal Execute()
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
}
