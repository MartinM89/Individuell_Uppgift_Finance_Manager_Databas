using Npgsql;

public abstract class Command
{
    public string Name { get; init; }

    public Command(string name)
    {
        this.Name = name;
    }

    public abstract Task Execute(NpgsqlConnection connection);

    public abstract string GetDescription();
}
