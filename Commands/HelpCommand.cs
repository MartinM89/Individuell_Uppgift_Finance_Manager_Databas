public class HelpCommand : Command
{
    public HelpCommand()
        : base("Help") { }

    public override string GetDescription()
    {
        return "Check help commands";
    }

    public override void RunCommand()
    {
        throw new NotImplementedException();
    }
}
