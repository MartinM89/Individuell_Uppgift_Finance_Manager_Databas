using Npgsql;

public class SaveAndExitCommand : Command
{
    public SaveAndExitCommand()
        : base("Save and Exit") { }

    public override string GetDescription()
    {
        return "Save and exit the program";
    }

    public override void Execute(NpgsqlConnection connection)
    {
        throw new NotImplementedException();
    }
}
