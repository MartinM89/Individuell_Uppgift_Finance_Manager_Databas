using Npgsql;

public class HelpCommand : Command
{
    public HelpCommand(GetManagers getManagers)
        : base('H', "Help", getManagers) { }

    public override string GetDescription()
    {
        return "Check help commands";
    }

    public override void Execute()
    {
        Console.WriteLine("Help Menu");

        GetManagers.UserMenuManager.ReturnToSameMenu();
    }
}
