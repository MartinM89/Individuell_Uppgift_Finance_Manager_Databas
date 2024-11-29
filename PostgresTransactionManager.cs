using Npgsql;

public class PostgresTransactionManager : ITransactionManager
{
    private NpgsqlConnection connection;
    Guid userId = LoginCommand.GetUserId();

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
                id INT NOT NULL,
                name VARCHAR NOT NULL,
                amount DECIMAL(10, 2) NOT NULL,
                date DATE DEFAULT NOW() NOT NULL,
                user_id UUID NOT NULL,
                PRIMARY KEY (id, user_id),
                FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
                );

                CREATE OR REPLACE FUNCTION assign_user_transaction_id()
                RETURNS TRIGGER AS $$
                BEGIN
                    NEW.id := COALESCE(
                        (SELECT MAX(id) + 1 FROM transactions WHERE user_id = NEW.user_id),
                        1
                    );

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;

                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1
                        FROM pg_trigger
                        WHERE tgname = 'before_transaction_insert'
                    ) THEN
                        CREATE TRIGGER before_transaction_insert
                        BEFORE INSERT ON transactions
                        FOR EACH ROW
                        EXECUTE FUNCTION assign_user_transaction_id();
                    END IF;
                END;
                $$;

                CREATE OR REPLACE FUNCTION reorder_user_transaction_ids()
                RETURNS TRIGGER AS $$
                BEGIN
                    WITH reordered AS (
                        SELECT id, ROW_NUMBER() OVER (ORDER BY date) AS new_id
                        FROM transactions
                        WHERE user_id = OLD.user_id
                    )
                    UPDATE transactions
                    SET id = reordered.new_id
                    FROM reordered
                    WHERE transactions.user_id = OLD.user_id
                    AND transactions.id = reordered.id;

                    RETURN NULL;
                END;
                $$ LANGUAGE plpgsql;

                DO $$
                BEGIN
                    IF NOT EXISTS (
                    SELECT 1
                    FROM pg_trigger
                    WHERE tgname = 'after_transaction_delete'
                    ) THEN
                        CREATE TRIGGER after_transaction_delete
                        AFTER DELETE ON transactions
                        FOR EACH ROW
                        EXECUTE FUNCTION reorder_user_transaction_ids();
                    END IF;
                END;
                $$;
            """;

        using var createTablesCmd = new NpgsqlCommand(createTablesSql, connection);
        createTablesCmd.ExecuteNonQuery();
    }

    public void SaveTransaction(Transaction transaction)
    {
        string insertTransactionSql = "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @amount, @user_id)";
        using var insertTransactionCmd = new NpgsqlCommand(insertTransactionSql, connection);
        insertTransactionCmd.Parameters.AddWithValue("@name", transaction.Name!);
        insertTransactionCmd.Parameters.AddWithValue("@amount", transaction.Amount);
        insertTransactionCmd.Parameters.AddWithValue("@user_id", transaction.UserId);

        insertTransactionCmd.ExecuteNonQuery();
    }

    public void DeleteTransaction(int deleteTransaction)
    {
        string deleteTransactionSql = "DELETE FROM transactions WHERE user_id = @user_id AND id = @id";
        using var deleteTransactionCmd = new NpgsqlCommand(deleteTransactionSql, connection);
        deleteTransactionCmd.Parameters.AddWithValue("@user_id", userId);
        deleteTransactionCmd.Parameters.AddWithValue("@id", deleteTransaction);

        deleteTransactionCmd.ExecuteNonQuery();
    }

    public void GetBalance()
    {
        throw new NotImplementedException();
    }

    public void GetTransactions()
    {
        string getTransactionsSql = "SELECT * FROM transactions WHERE user_id = @user_id";
        using var getTransactionsCmd = new NpgsqlCommand(getTransactionsSql, connection);
        getTransactionsCmd.Parameters.AddWithValue("@user_id", userId);

        NpgsqlDataReader reader = getTransactionsCmd.ExecuteReader();

        while (reader.Read())
        {
            int userId = reader.GetInt32(0);
            string name = reader.GetString(1);
            decimal amount = reader.GetDecimal(2);
            DateTime date = reader.GetDateTime(3);

            Console.WriteLine($"{userId} {name} {amount} {date}");
        }
        PressKeyToContinue.Execute();
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
