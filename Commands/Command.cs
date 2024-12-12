public abstract class Command
{
    public char Shortcut { get; init; }
    public string Name { get; init; }
    protected GetManagers GetManagers { get; init; }

    public Command(char shortcut, string name, GetManagers getManagers)
    {
        this.Shortcut = shortcut;
        this.Name = name;
        this.GetManagers = getManagers;
    }

    public abstract Task Execute();

    public abstract string GetDescription();
}
