public abstract class Command
{
    public char Shortcut { get; init; }
    public string Name { get; init; }
    protected GetManagers GetManagers { get; init; }

    public Command(char shortcut, string name, GetManagers getManagers)
    {
        Shortcut = shortcut;
        Name = name;
        GetManagers = getManagers;
    }

    public abstract Task Execute();

    public abstract string GetDescription();
}
