public class CheckBalanceCommand : Command
{
    public CheckBalanceCommand()
        : base("Check Balance") { }

    public override string GetDescription()
    {
        return "Check your current balance";
    }

    public override void RunCommand()
    {
        throw new NotImplementedException();
    }
}
