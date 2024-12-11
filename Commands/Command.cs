public abstract class Command
{
    public string Name { get; init; }
    protected GetManagers GetManagers { get; init; }

    public Command(string name, GetManagers getManagers)
    {
        this.Name = name;
        this.GetManagers = getManagers;
    }

    public abstract Task Execute();

    public abstract string GetDescription();
}
