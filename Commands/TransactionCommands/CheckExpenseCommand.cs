using Npgsql;

public class CheckExpenseCommand : Command
{
    public CheckExpenseCommand()
        : base("Check Expenses") { }

    public override string GetDescription()
    {
        return "Check your expenses";
    }

    public override Task Execute(NpgsqlConnection connection)
    {
        throw new NotImplementedException();
    }
}
