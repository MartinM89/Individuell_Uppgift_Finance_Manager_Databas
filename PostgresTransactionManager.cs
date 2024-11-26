using Npgsql;

public class PostgresTransactionManager : ITransactionManager
{
    private NpgsqlConnection connection;

    public PostgresTransactionManager()
    {
        string connectionString = DatabaseConnection.GetConnectionString();

        this.connection = new NpgsqlConnection(connectionString);
        connection.Open();

        var createTablesSql = """
                CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

                CREATE TABLE IF NOT EXISTS users (
                id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
                username VARCHAR(25) UNIQUE NOT NULL,
                password_hash VARCHAR(100) NOT NULL,
                password_salt VARCHAR NOT NULL
            );

                CREATE TABLE IF NOT EXISTS transactions (
                id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
                name VARCHAR NOT NULL,
                amount DECIMAL(10, 2) NOT NULL,
                date DATE DEFAULT NOW() NOT NULL,
                user_id UUID NOT NULL,
                FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
            );
            """;

        using var createTablesCmd = new NpgsqlCommand(createTablesSql, connection);
        createTablesCmd.ExecuteNonQuery();
    }

    public void SaveTransaction(Transaction transaction)
    {
        var insertTransactionSql = "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @amount, @user_id)";
        using var insertTransactionCmd = new NpgsqlCommand(insertTransactionSql, connection);
        insertTransactionCmd.Parameters.AddWithValue("@name", transaction.Name!);
        insertTransactionCmd.Parameters.AddWithValue("@amount", transaction.Amount);
        insertTransactionCmd.Parameters.AddWithValue("@user_id", transaction.UserId);

        insertTransactionCmd.ExecuteNonQuery();
    }

    public void DeleteTransaction(int id)
    {
        throw new NotImplementedException();
    }

    public void GetBalance()
    {
        throw new NotImplementedException();
    }

    public List<Transaction> GetTransactions()
    {
        throw new NotImplementedException();
    }

    public void LoadTransactions(List<Transaction> loadTransactions)
    {
        throw new NotImplementedException();
    }

    public void PrintTransactions()
    {
        throw new NotImplementedException();
    }
}
