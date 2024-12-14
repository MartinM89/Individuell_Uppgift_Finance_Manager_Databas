using Npgsql;

public class CheckExpenseCommand : Command
{
    public CheckExpenseCommand(GetManagers getManagers)
        : base('E', "Expense", getManagers) { }

    public override string GetDescription()
    {
        return "Check your expenses";
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}
