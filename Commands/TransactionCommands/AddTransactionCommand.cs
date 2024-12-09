using Npgsql;

public class AddTransactionCommand : Command
{
    public AddTransactionCommand(NpgsqlConnection connection, IAccountManager accountManager, IMenuManager menuManager, ITransactionManager transactionManager)
        : base("A", connection, accountManager, menuManager, transactionManager) { }

    public override string GetDescription()
    {
        return "Adds a transaction";
    }

    public override async Task Execute()
    {
        Console.Clear();

        DateTime transactionDate = DateTime.Now;

        string capitalizedTransactionName;

        while (true)
        {
            Console.Clear();
            Console.Write("Enter name: ");
            string? transactionName = Console.ReadLine();
            // csharpier-ignore
            if (string.IsNullOrEmpty(transactionName)) { return; }

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

            capitalizedTransactionName = char.ToUpper(transactionName[0]) + transactionName[1..];
            break;
        }

        decimal transactionValue;

        while (true)
        {
            Console.Clear();
            Console.Write("Enter amount: ");
            string? transactionValueString = Console.ReadLine();
            // csharpier-ignore
            if (string.IsNullOrEmpty(transactionValueString)) { return; }

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

            break;
        }

        Transaction transaction = new(1, capitalizedTransactionName, transactionValue, transactionDate, PostgresAccountManager.LoggedInUserId);

        PostgresTransactionManager postgresTransactionManager = new(connection);

        await postgresTransactionManager.AddTransaction(transaction);

        Console.Clear();
        Console.WriteLine("The following transaction has been added:");
        TransactionTable.GetTransactionTableTop();
        TransactionTable.GetSingleRowTransactionTableCenter(transaction);
        TransactionTable.GetTransactionsTableBottom();

        PressKeyToContinue.Execute();

        menuManager.ReturnToSameMenu();
    }
}
