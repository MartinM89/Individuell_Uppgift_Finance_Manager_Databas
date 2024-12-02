using Npgsql;

public class CheckBalanceCommand : Command
{
    public CheckBalanceCommand()
        : base("Check Balance") { }

    public override string GetDescription()
    {
        return "Check your current balance";
    }

    public override void Execute(NpgsqlConnection connection)
    {
        throw new NotImplementedException();
    }
}
