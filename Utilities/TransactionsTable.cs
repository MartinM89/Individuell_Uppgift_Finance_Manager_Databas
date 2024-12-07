using Individuell_Uppgift.Utilities;

public static class TransactionTable
{
    private static readonly string id = "Id";
    private static readonly string date = "Date";
    private static readonly string transactionName = "Transaction Name";
    private static readonly string amount = "Amount";

    public const int idWidth = 3;
    public const int dateWidth = 11;
    public const int transactionNameWidth = 21;
    public const int amountWidth = 13;
    public const int columns = 4;
    public const int padding = columns * 2;

    public static void GetTransactionTableTop()
    {
        int tableLength = idWidth + dateWidth + transactionNameWidth + amountWidth + padding + columns;
        string repeatedUnderscore = string.Concat(Enumerable.Repeat(" _", tableLength / 2));
        string repeatedUpperUnderscore = string.Concat(Enumerable.Repeat(" ═", tableLength / 2));

        Console.WriteLine(repeatedUnderscore);
        Console.WriteLine($"| {id, idWidth} | {date, -dateWidth} | {transactionName, -transactionNameWidth} | {amount, amountWidth} |");
        Console.WriteLine(repeatedUpperUnderscore);
    }

    public static void GetSingleRowTransactionTableCenter(Transaction transaction)
    {
        Console.Write(transaction);
        ChangeColor.TextColorGreen($"{transaction.Amount, amountWidth:N2}");
        Console.WriteLine(" |");
    }

    public static void GetMultipleRowsTransactionTableCenter(List<Transaction> transactions)
    {
        foreach (Transaction transaction in transactions)
        {
            Console.Write(transaction);

            if (transaction.Amount > 0)
            {
                ChangeColor.TextColorGreen($"{transaction.Amount, amountWidth:N2}");
                Console.WriteLine(" |");
            }
            else
            {
                ChangeColor.TextColorRed($"{transaction.Amount, amountWidth:N2}");
                Console.WriteLine(" |");
            }
        }
    }

    public static void GetTransactionsTableBottom()
    {
        int tableLength = idWidth + dateWidth + transactionNameWidth + amountWidth + padding + columns;
        string repeatedUpperscore = string.Concat(Enumerable.Repeat(" ═", tableLength / 2));

        Console.WriteLine(repeatedUpperscore);
    }
}
