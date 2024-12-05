using Npgsql;

public class ExitCommand : Command
{
    public ExitCommand()
        : base("Save and Exit") { }

    public override string GetDescription()
    {
        return "Save and exit the program";
    }

    public override Task Execute(NpgsqlConnection connection)
    {
        throw new NotImplementedException();
    }
}
