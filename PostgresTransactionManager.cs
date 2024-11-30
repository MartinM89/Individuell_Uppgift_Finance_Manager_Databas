using Npgsql;
using NpgsqlTypes;

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
        using NpgsqlCommand insertTransactionCmd = new(insertTransactionSql, connection);
        insertTransactionCmd.Parameters.AddWithValue("@name", transaction.Name!);
        insertTransactionCmd.Parameters.AddWithValue("@amount", transaction.Amount);
        insertTransactionCmd.Parameters.AddWithValue("@user_id", transaction.UserId);

        insertTransactionCmd.ExecuteNonQuery();
    }

    public void DeleteTransaction(int deleteTransaction)
    {
        string deleteTransactionSql = "DELETE FROM transactions WHERE user_id = @user_id AND id = @id";
        using NpgsqlCommand deleteTransactionCmd = new(deleteTransactionSql, connection);
        deleteTransactionCmd.Parameters.AddWithValue("@user_id", userId);
        deleteTransactionCmd.Parameters.AddWithValue("@id", deleteTransaction);

        deleteTransactionCmd.ExecuteNonQuery();
    }

    public void GetBalance()
    {
        string getBalance = "SELECT * FROM transactions WHERE user_id = @user_id";
        using NpgsqlCommand getBalanceCmd = new(getBalance, connection);
        getBalanceCmd.Parameters.AddWithValue("@user_id", userId);

        NpgsqlDataReader reader = getBalanceCmd.ExecuteReader();

        decimal totalBalance = 0;

        while (reader.Read())
        {
            totalBalance += reader.GetDecimal(2);
        }

        Console.WriteLine($"Your total balance is {totalBalance}"); // Need to remove and return instead
        PressKeyToContinue.Execute();
    }

    public void GetAllTransactions()
    {
        string getAllTransactions = "SELECT * FROM transactions WHERE user_id = @user_id";
        using NpgsqlCommand getAllTransactionsCmd = new(getAllTransactions, connection);
        getAllTransactionsCmd.Parameters.AddWithValue("@user_id", userId);

        NpgsqlDataReader reader = getAllTransactionsCmd.ExecuteReader();

        while (reader.Read())
        {
            int userId = reader.GetInt32(0);
            string name = reader.GetString(1);
            decimal amount = reader.GetDecimal(2);
            DateTime date = reader.GetDateTime(3);

            Console.WriteLine($"{userId} {name} {amount} {date:dd MMM}"); // Remove and return transactions object instead
        }
        PressKeyToContinue.Execute();
    }

    public Transaction GetTransactionsByDay(int dayOfMonth, char transactionType)
    {
        string getTransactionsByDay = $"""
            SELECT * FROM transactions
            WHERE user_id = @user_id
            AND @dayOfMonth IS NOT NULL AND EXTRACT(DAY FROM date) = @dayOfMonth
            AND amount {transactionType} 0
            """;
        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByDay, connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", userId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@dayOfMonth", dayOfMonth);
        // getTransactionsByDayCmd.Parameters.AddWithValue("@transactionType", transactionType); // Doesn't work?

        NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader();

        string name = string.Empty;
        decimal amount = 0;
        DateTime date = DateTime.Now;

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            name = reader.GetString(1);
            amount = reader.GetDecimal(2);
            date = reader.GetDateTime(3);

            Console.WriteLine($"{date:dd MMM} {name} {amount}"); // Remove and return transactions object instead
        }

        Transaction transaction = new(name, amount, date, userId);

        return transaction;
    }

    public Transaction GetTransactionsByWeek(int weekNumber, char transactionType)
    {
        string getTransactionsByDay = $"""
            SELECT * FROM transactions
            WHERE user_id = @user_id
            AND @weekNumber IS NOT NULL AND EXTRACT(WEEK FROM date) = @weekNumber
            AND amount {transactionType} 0
            """;
        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByDay, connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", userId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@weekNumber", weekNumber);

        NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader();

        string name = string.Empty;
        decimal amount = 0;
        DateTime date = DateTime.Now;

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            name = reader.GetString(1);
            amount = reader.GetDecimal(2);
            date = reader.GetDateTime(3);

            Console.WriteLine($"{date:dd MMM} {name} {amount}"); // Remove and return transactions object instead
        }

        Transaction transaction = new(name, amount, date, userId);

        return transaction;
    }

    public Transaction GetTransactionsByMonth(int month, char transactionType)
    {
        string getTransactionsByDay = $"""
            SELECT * FROM transactions
            WHERE user_id = @user_id
            AND @month IS NOT NULL AND EXTRACT(MONTH FROM date) = @month
            AND amount {transactionType} 0
            """;
        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByDay, connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", userId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@month", month);

        NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader();

        string name = string.Empty;
        decimal amount = 0;
        DateTime date = DateTime.Now;

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            name = reader.GetString(1);
            amount = reader.GetDecimal(2);
            date = reader.GetDateTime(3);

            Console.WriteLine($"{date:dd MMM} {name} {amount}"); // Remove and return transactions object instead
        }

        Transaction transaction = new(name, amount, date, userId);

        return transaction;
    }

    public Transaction GetTransactionsByYear(int year, char transactionType)
    {
        string getTransactionsByDay = $"""
            SELECT * FROM transactions
            WHERE user_id = @user_id
            AND @year IS NOT NULL AND EXTRACT(YEAR FROM date) = @year
            AND amount {transactionType} 0
            """;
        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByDay, connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", userId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@year", year);

        NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader();

        string name = string.Empty;
        decimal amount = 0;
        DateTime date = DateTime.Now;

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            name = reader.GetString(1);
            amount = reader.GetDecimal(2);
            date = reader.GetDateTime(3);

            Console.WriteLine($"{date:dd MMM} {name} {amount}"); // Remove and return transactions object instead
        }

        Transaction transaction = new(name, amount, date, userId);

        return transaction;
    }
}
