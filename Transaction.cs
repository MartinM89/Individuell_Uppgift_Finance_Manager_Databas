public class Transaction
{
    public int Id { get; private set; }
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
}
