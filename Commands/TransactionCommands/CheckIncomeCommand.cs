public class CheckIncomeCommand : Command
{
    public CheckIncomeCommand()
        : base("Check Income") { }

    public override string GetDescription()
    {
        return "Check your income";
    }

    public override void RunCommand()
    {
        throw new NotImplementedException();
    }
}
