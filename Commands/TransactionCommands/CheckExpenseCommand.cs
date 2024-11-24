public class CheckExpenseCommand : Command
{
    public CheckExpenseCommand()
        : base("Check Expenses") { }

    public override string GetDescription()
    {
        return "Check your expenses";
    }

    public override void RunCommand()
    {
        throw new NotImplementedException();
    }
}
