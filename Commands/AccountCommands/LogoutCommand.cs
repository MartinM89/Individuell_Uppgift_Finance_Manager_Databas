public class LogoutCommand : Command
{
    public LogoutCommand(GetManagers getManagers)
        : base('L', "Log out", getManagers) { }

    public override string GetDescription()
    {
        return "Logout the current user";
    }

    public override Task Execute()
    {
        Console.Clear();
        Console.WriteLine("Thank you for using your personal finance app.");
        PressKeyToContinue.Execute();

        PostgresAccountManager.SetLoggedInUserIdToEmpty();

        GetManagers.UserMenuManager.SetMenu(new LoginMenu(GetManagers));

        return Task.CompletedTask;
    }
}
