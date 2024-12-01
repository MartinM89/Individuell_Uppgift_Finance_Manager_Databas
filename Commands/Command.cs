using Npgsql;

public abstract class Command
{
    public string Name { get; init; }

    public Command(string name)
    {
        this.Name = name;
    }

    public abstract void RunCommand(NpgsqlConnection connection);

    public abstract string GetDescription();
}
