public class Transaction
{
    public DateTime Date { get; private set; }
    public string? Name { get; private set; }
    public decimal Amount { get; private set; }
    public Guid UserId { get; private set; }

    public Transaction(DateTime date, string name, decimal amount, Guid userId)
    {
        this.Date = date;
        this.Name = name;
        this.Amount = amount;
        this.UserId = userId;
    }

    // public override string ToString()
    // {
    //     int longestName = TransactionManager.LongestNameLength();
    //     int longestAmount = TransactionManager.LongestAmountLength();

    //     string transactionName = Name!.PadRight(longestName);
    //     string transactionAmount = Math.Round(Amount, 2).ToString("F2") + ":-";
    //     transactionAmount = transactionAmount.PadLeft(longestAmount);

    //     return $"| {Date:yyyy MMM dd} | {Name} | {Amount} |";
    // }
}
