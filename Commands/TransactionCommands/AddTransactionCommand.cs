public class AddTransactionCommand : Command
{
    public AddTransactionCommand()
        : base("Add Transaction") { }

    public override string GetDescription()
    {
        return "Adds a transaction";
    }

    public override void RunCommand()
    {
        throw new NotImplementedException();
    }
}
