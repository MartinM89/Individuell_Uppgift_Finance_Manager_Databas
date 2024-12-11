using Npgsql;

public class LogoutCommand : Command
{
    public LogoutCommand(GetManagers getManagers)
        : base("L", getManagers) { }

    public override string GetDescription()
    {
        return "Log out from current user";
    }

    public override Task Execute()
    {
        Console.Clear();
        Console.WriteLine("Thank you for using your personal finance app.");
        PressKeyToContinue.Execute();

        GetManagers.UserMenuManager.SetMenu(new LoginMenu(GetManagers));

        return Task.CompletedTask;
    }
}
