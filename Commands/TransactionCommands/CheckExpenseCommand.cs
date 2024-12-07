using Npgsql;

public class CheckExpenseCommand : Command
{
    NpgsqlConnection connection;

    public CheckExpenseCommand(NpgsqlConnection connection)
        : base(connection, "E")
    {
        this.connection = connection;
    }

    public override string GetDescription()
    {
        return "Check your expenses";
    }

    public override Task Execute()
    {
        throw new NotImplementedException();
    }
}
