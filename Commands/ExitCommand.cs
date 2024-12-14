using Npgsql;

public class ExitCommand : Command
{
    public ExitCommand(GetManagers getManagers)
        : base('E', "Exit", getManagers) { }

    public override string GetDescription()
    {
        return "Exit the program";
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}
