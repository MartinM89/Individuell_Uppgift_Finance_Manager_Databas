public class CheckBalancePerUserCommand : Command
{
    public CheckBalancePerUserCommand(GetManagers getManagers)
        : base('T', "Total balance by user", getManagers) { }

    public override string GetDescription()
    {
        return "Check the balance on a per user basis";
    }

    public override async Task Execute()
    {
        await GetManagers.TransactionManager.ShowBalancePerUser();
        PressKeyToContinue.Execute();

        return;
    }
}
