using Npgsql;

public class AddTransactionCommand : Command
{
    public AddTransactionCommand()
        : base("Add Transaction") { }

    public override string GetDescription()
    {
        return "Adds a transaction";
    }

    public override void Execute(NpgsqlConnection connection)
    {
        Console.Clear();

        DateTime transactionDate = DateTime.Now;

        string capitalizedTransactionName;

        while (true)
        {
            Console.Clear();
            Console.Write("Enter name: ");
            string transactionName = Console.ReadLine()!;

            if (string.IsNullOrEmpty(transactionName))
            {
                return;
            }

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

            if (transactionName.Length! < 3 || transactionName.Length! > 20)
            {
                Console.Clear();
                Console.WriteLine("Invalid Input. Name must be between 3 to 20 characters long.");
                PressKeyToContinue.Execute();
                continue;
            }

            capitalizedTransactionName = char.ToUpper(transactionName[0]) + transactionName[1..];
            break;
        }

        decimal transactionValue;

        while (true)
        {
            Console.Clear();
            Console.Write("Enter amount: ");
            string transactionValueString = Console.ReadLine()!;

            if (string.IsNullOrEmpty(transactionValueString))
            {
                return;
            }

            transactionValueString = transactionValueString.Replace('.', ',');

            if (transactionValueString.Length > 10 || !decimal.TryParse(transactionValueString, out transactionValue))
            {
                Console.Clear();
                Console.WriteLine("Invalid Input. Amount must be only numbers and not exceed 10 numbers long.");
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

            break;
        }

        Transaction transaction = new Transaction(capitalizedTransactionName, transactionValue, transactionDate, LoginCommand.Id) { };

        var transactionManager = new PostgresTransactionManager(connection);

        transactionManager.SaveTransaction(transaction);

        Console.Clear();
        Console.WriteLine($"The following transaction has been added:\n| {transaction.Date:yyyy MMM dd} | {transaction.Name} | {transaction.Amount:F2} |");
        PressKeyToContinue.Execute();
    }
}
