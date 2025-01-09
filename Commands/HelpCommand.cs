using Npgsql;

public class HelpCommand : Command
{
    public HelpCommand(GetManagers getManagers)
        : base('H', "Help Page", getManagers) { }

    public override string GetDescription()
    {
        return "Check help commands";
    }

    public override Task Execute()
    {
        GetManagers.UserMenuManager.GetMenu().HelpMenu();

        GetManagers.UserMenuManager.ReturnToSameMenu();

        return Task.CompletedTask;
    }
}
