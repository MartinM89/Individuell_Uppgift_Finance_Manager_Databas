using Npgsql;

public class CheckExpenseCommand : Command
{
    public CheckExpenseCommand(GetManagers getManagers)
        : base("E", getManagers) { }

    public override string GetDescription()
    {
        return "Check your expenses";
    }

    public override Task Execute()
    {
        throw new NotImplementedException();
    }
}
