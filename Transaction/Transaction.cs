using System.Drawing;
using Individuell_Uppgift.Utilities;

public class Transaction
{
    public int Id { get; private set; }
    public DateTime Date { get; private set; }
    public string? Name { get; private set; }
    public decimal Amount { get; private set; }
    public Guid UserId { get; private set; }

    public Transaction(int id, string name, decimal amount, DateTime date, Guid userId)
    {
        this.Id = id;
        this.Name = name;
        this.Amount = amount;
        this.Date = date;
        this.UserId = userId;
    }

    public override string ToString()
    {
        // return $"| {Id, 3} | {Date:yyyy MMM dd} | {Name, -21} | {Amount, 13:N2} |";
        return $"| {Id, TransactionTable.idWidth} | {Date:yyyy MMM dd} | {Name, -TransactionTable.transactionNameWidth} | ";
    }
}
