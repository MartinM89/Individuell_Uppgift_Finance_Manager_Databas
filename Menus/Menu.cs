namespace Individuell_Uppgift.Menus;

public abstract class Menu
{
    private readonly List<Command> commands = [];

    public void AddCommand(Command command)
    {
        this.commands.Add(command);
    }

    public void ExecuteCommand(string inputCommand)
    {
        foreach (Command command in commands)
        {
            if (command.Name.Equals(inputCommand))
            {
                command.Execute(inputCommand);
                return;
            }
        }

        throw new ArgumentException("Command not found.");
    }

    public abstract void Display();
}
