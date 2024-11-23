public class SaveAndExitCommand : Command
{
    public SaveAndExitCommand()
        : base("Save and Exit") { }

    public override string GetDescription()
    {
        return "Save and exit the program";
    }

    public override void RunCommand()
    {
        throw new NotImplementedException();
    }
}
