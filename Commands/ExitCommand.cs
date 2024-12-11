using Npgsql;

public class ExitCommand : Command
{
    public ExitCommand(GetManagers getManagers)
        : base("E", getManagers) { }

    public override string GetDescription()
    {
        return "Exit the program";
    }

    public override Task Execute()
    {
        throw new NotImplementedException();
    }
}
