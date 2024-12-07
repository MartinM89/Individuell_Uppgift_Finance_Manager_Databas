using Npgsql;

public abstract class Command
{
    public string Name { get; init; }

    public Command(NpgsqlConnection connection, string name)
    {
        this.Name = name;
    }

    public abstract Task Execute();

    public abstract string GetDescription();
}
