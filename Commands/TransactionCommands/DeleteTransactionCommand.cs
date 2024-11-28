public class DeleteTransactionCommand : Command
{
    public DeleteTransactionCommand()
        : base("Delete Transaction") { }

    public override string GetDescription()
    {
        return "Delete a transaction";
    }

    public override void RunCommand()
    {
        Console.Clear();

        Console.Write("What transaction do you wish to delete? ");
        int transactionToDelete = int.Parse(Console.ReadLine()!);
    }
}
