using Npgsql;

public class HelpCommand : Command
{
    NpgsqlConnection connection;

    public HelpCommand(NpgsqlConnection connection)
        : base(connection, "H")
    {
        this.connection = connection;
    }

    public override string GetDescription()
    {
        return "Check help commands";
    }

    public override Task Execute()
    {
        throw new NotImplementedException();
    }
}
