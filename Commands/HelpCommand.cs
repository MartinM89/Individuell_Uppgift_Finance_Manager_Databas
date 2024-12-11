using Npgsql;

public class HelpCommand : Command
{
    public HelpCommand(GetManagers getManagers)
        : base("H", getManagers) { }

    public override string GetDescription()
    {
        return "Check help commands";
    }

    public override Task Execute()
    {
        Console.WriteLine("Help Menu");

        GetManagers.UserMenuManager.ReturnToSameMenu();

        return Task.CompletedTask;
    }
}
