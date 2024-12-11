public class GetTransactionAmount
{
    public static decimal Execute()
    {
        decimal transactionValue;

        while (true)
        {
            Console.Clear();
            Console.Write("Enter amount: ");
            string? transactionValueString = Console.ReadLine();
            // csharpier-ignore
            if (string.IsNullOrEmpty(transactionValueString)) { return 0; }

            transactionValueString = transactionValueString.Replace('.', ',');

            if (!decimal.TryParse(transactionValueString, out transactionValue))
            {
                Console.Clear();
                Console.WriteLine("Invalid Input. Amount must only be numbers.");
                PressKeyToContinue.Execute();
                continue;
            }

            if (transactionValueString.Length > TransactionTable.amountWidth - 5)
            {
                Console.Clear();
                Console.WriteLine("Invalid Input. Amount can't exceed 8 numbers.");
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

            Console.WriteLine("Decimal:" + transactionValue);
            PressKeyToContinue.Execute();

            return transactionValue;
        }
    }
}
