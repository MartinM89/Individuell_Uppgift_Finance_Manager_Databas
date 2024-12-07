namespace Individuell_Uppgift.Menus;

public abstract class Menu
{
    private List<Command> commands = [];

    public void AddCommand(Command command)
    {
        this.commands.Add(command);
    }

    public void ExecuteCommand(string userChoice)
    {
        try
        {
            foreach (Command command in commands)
            {
                if (command.Name.Equals(userChoice))
                {
                    command.Execute();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Command not found. | {ex.Message}");
        }
    }

    public abstract void Display();
}
