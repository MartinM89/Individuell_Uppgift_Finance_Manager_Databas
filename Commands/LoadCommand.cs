using Npgsql;

public class LoadCommand : Command
{
    public LoadCommand()
        : base("Load Transactions") { }

    public override string GetDescription()
    {
        return "Load your transactions";
    }

    public override void RunCommand(NpgsqlConnection connection)
    {
        throw new NotImplementedException();
    }
}
