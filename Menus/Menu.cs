using Individuell_Uppgift.Utilities;

namespace Individuell_Uppgift.Menus;

public abstract class Menu
{
    protected List<Command> commands = [];

    public void AddCommand(Command command)
    {
        this.commands.Add(command);
    }

    public async Task ExecuteCommand(char userChoiceChar)
    {
        try
        {
            foreach (Command command in commands)
            {
                if (command.Shortcut.Equals(userChoiceChar))
                {
                    await command.Execute();
                    return;
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Command not found. | {ex.Message}");
            return;
        }
    }

    public abstract void Display();

    public virtual void HelpMenu()
    {
        Console.Clear();

        Console.WriteLine("Help Menu:\n");

        foreach (Command command in commands)
        {
            if (command.Shortcut == 'H')
            {
                continue;
            }

            ChangeColor.TextColorGreen($"[{command.Name}]");
            Console.WriteLine($" - {command.GetDescription()}");
        }

        Console.ReadKey();
    }
}
