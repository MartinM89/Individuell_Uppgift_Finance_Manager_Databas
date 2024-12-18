public class AdminAddTransactionCommand : Command
{
    public AdminAddTransactionCommand(GetManagers getManagers)
        : base('A', "Add Trasaction", getManagers) { }

    public override string GetDescription()
    {
        return "Add transaction to any user (Admin only)";
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}
// ????
