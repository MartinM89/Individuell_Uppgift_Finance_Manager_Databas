using Npgsql;

public class CheckBalanceCommand : Command
{
    public CheckBalanceCommand()
        : base("Check Balance") { }

    public override string GetDescription()
    {
        return "Check your current balance";
    }

    public override async Task Execute(NpgsqlConnection connection)
    {
        PostgresTransactionManager postgresTransactionManager = new(connection);

        await postgresTransactionManager.GetBalance();
    }
}
