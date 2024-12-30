namespace Individuell_Uppgift.Menus;

public abstract class Menu
{
    private List<Command> commands = [];

    public void AddCommand(Command command)
    {
        this.commands.Add(command);
    }

    public async Task ExecuteCommand(string userChoice)
    {
        _ = char.TryParse(userChoice, out char userChoiceChar);

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
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Command not found. | {ex.Message}");
        }
    }

    public abstract void Display();
}
