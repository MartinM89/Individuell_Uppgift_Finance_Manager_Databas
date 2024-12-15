using Individuell_Uppgift;
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
        Console.Clear();
        Console.WriteLine("Thank you for using your personal finance program.");
        PressKeyToContinue.Execute();
        Console.Clear();
        Program.run = false;
    }
}
