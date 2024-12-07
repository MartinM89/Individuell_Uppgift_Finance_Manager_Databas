using Npgsql;

public class ExitCommand : Command
{
    NpgsqlConnection connection;

    public ExitCommand(NpgsqlConnection connection)
        : base(connection, "E")
    {
        this.connection = connection;
    }

    public override string GetDescription()
    {
        return "Exit the program";
    }

    public override Task Execute()
    {
        throw new NotImplementedException();
    }
}
