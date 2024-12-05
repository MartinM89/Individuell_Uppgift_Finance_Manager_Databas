using Npgsql;

public class PostgresTransactionManager : ITransactionManager
{
    private readonly NpgsqlConnection Connection;

    public PostgresTransactionManager(NpgsqlConnection connection)
    {
        Connection = connection;

        #region CreateTableFunctionTriggersQuery

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

                CREATE OR REPLACE FUNCTION reorder_all_user_transactions()
                RETURNS VOID AS $$
                DECLARE
                    temp_table_name TEXT;
                BEGIN
                    temp_table_name := 'temp_reordered_transactions_' || pg_backend_pid();

                    EXECUTE format('
                        CREATE TEMPORARY TABLE %I AS
                        SELECT
                            ROW_NUMBER() OVER (PARTITION BY user_id ORDER BY date) AS new_id,
                            name, amount, date, user_id
                        FROM transactions
                    ', temp_table_name);

                    DELETE FROM transactions;

                    EXECUTE format('
                        INSERT INTO transactions (id, name, amount, date, user_id)
                        SELECT
                            new_id, name, amount, date, user_id
                        FROM %I
                    ', temp_table_name);

                    EXECUTE format('DROP TABLE %I', temp_table_name);
                END;
                $$ LANGUAGE plpgsql;

                SELECT reorder_all_user_transactions();
            """;

        #endregion

        using NpgsqlCommand createTablesCmd = new(createTablesSql, Connection);
        createTablesCmd.ExecuteNonQuery();
    }

    public void AddTransaction(Transaction transaction)
    {
        string insertTransactionSql = "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @amount, @user_id)";
        using NpgsqlCommand insertTransactionCmd = new(insertTransactionSql, Connection);
        insertTransactionCmd.Parameters.AddWithValue("@name", transaction.Name!);
        insertTransactionCmd.Parameters.AddWithValue("@amount", transaction.Amount);
        insertTransactionCmd.Parameters.AddWithValue("@user_id", transaction.UserId);

        insertTransactionCmd.ExecuteNonQuery();
    }

    public void DeleteTransaction(int transactionToDelete)
    {
        string deleteTransactionSql = "DELETE FROM transactions WHERE user_id = @user_id AND id = @id";
        using NpgsqlCommand deleteTransactionCmd = new(deleteTransactionSql, Connection);
        deleteTransactionCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);
        deleteTransactionCmd.Parameters.AddWithValue("@id", transactionToDelete);

        deleteTransactionCmd.ExecuteNonQuery();
    }

    public decimal GetBalance()
    {
        string getBalanceSql = "SELECT * FROM transactions WHERE user_id = @user_id";
        using NpgsqlCommand getBalanceCmd = new(getBalanceSql, Connection);
        getBalanceCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);

        using NpgsqlDataReader reader = getBalanceCmd.ExecuteReader();

        decimal totalBalance = 0;

        while (reader.Read())
        {
            totalBalance += reader.GetDecimal(2);
        }

        Console.WriteLine($"Your total balance is {totalBalance}"); // Remove and return balance as decimal instead
        PressKeyToContinue.Execute();

        return totalBalance;
    }

    public List<Transaction> GetAllTransactions()
    {
        string getAllTransactionsSql = "SELECT * FROM transactions WHERE user_id = @user_id";
        using NpgsqlCommand getAllTransactionsCmd = new(getAllTransactionsSql, Connection);
        getAllTransactionsCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);

        using NpgsqlDataReader reader = getAllTransactionsCmd.ExecuteReader();

        List<Transaction> transactions = [];

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            decimal amount = reader.GetDecimal(2);
            DateTime date = reader.GetDateTime(3);
            // Guid userId = reader.GetGuid(4); // Better to get this or use static LoggedInUserId?

            Transaction transaction = new(id, name, amount, date, PostgresAccountManager.LoggedInUserId);

            Console.WriteLine($"{id} {name} {amount} {date:dd MMM}"); // Remove and return transactions object instead
        }

        PressKeyToContinue.Execute();

        return transactions;
    }

    public List<Transaction> GetTransactionsByDay(int dayOfMonth, char transactionType) // Send bool instead of char to trigger tenerary interator
    {
        string getTransactionsByDaySql = $"""
            SELECT * FROM transactions
            WHERE user_id = @user_id
            AND @dayOfMonth IS NOT NULL AND EXTRACT(DAY FROM date) = @dayOfMonth
            AND amount {transactionType} 0
            """; // Use tenerary iterator here for '>' / '<'
        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByDaySql, Connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@dayOfMonth", dayOfMonth);

        using NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader();

        List<Transaction> transactions = [];

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            decimal amount = reader.GetDecimal(2);
            DateTime date = reader.GetDateTime(3);

            Transaction transaction = new(id, name, amount, date, PostgresAccountManager.LoggedInUserId);

            Console.WriteLine($"{date:dd MMM yyyy} {name} {amount}"); // Remove and return transactions list instead
        }

        return transactions;
    }

    public List<Transaction> GetTransactionsByWeek(int weekNumber, char transactionType)
    {
        string getTransactionsByWeekSql = $"""
            SELECT * FROM transactions
            WHERE user_id = @user_id
            AND @weekNumber IS NOT NULL AND EXTRACT(WEEK FROM date) = @weekNumber
            AND amount {transactionType} 0
            """;
        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByWeekSql, Connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@weekNumber", weekNumber);

        using NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader();

        List<Transaction> transactions = [];

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            decimal amount = reader.GetDecimal(2);
            DateTime date = reader.GetDateTime(3);

            Transaction transaction = new(id, name, amount, date, PostgresAccountManager.LoggedInUserId);

            Console.WriteLine($"{date:dd MMM yyyy} {name} {amount}"); // Remove and return transactions list instead
        }

        return transactions;
    }

    public List<Transaction> GetTransactionsByMonth(int month, char transactionType)
    {
        string getTransactionsByMonthSql = $"""
            SELECT * FROM transactions
            WHERE user_id = @user_id
            AND @month IS NOT NULL AND EXTRACT(MONTH FROM date) = @month
            AND amount {transactionType} 0
            """;
        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByMonthSql, Connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@month", month);

        using NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader();

        List<Transaction> transactions = [];

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            decimal amount = reader.GetDecimal(2);
            DateTime date = reader.GetDateTime(3);

            Transaction transaction = new(id, name, amount, date, PostgresAccountManager.LoggedInUserId);

            Console.WriteLine($"{date:dd MMM yyyy} {name} {amount}"); // Remove and return transactions list instead
        }

        return transactions;
    }

    public List<Transaction> GetTransactionsByYear(int year, char transactionType)
    {
        string getTransactionsByYearSql = $"""
            SELECT * FROM transactions
            WHERE user_id = @user_id
            AND @year IS NOT NULL AND EXTRACT(YEAR FROM date) = @year
            AND amount {transactionType} 0
            """;
        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByYearSql, Connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@year", year);

        using NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader();

        List<Transaction> transactions = [];

        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            decimal amount = reader.GetDecimal(2);
            DateTime date = reader.GetDateTime(3);

            Transaction transaction = new(id, name, amount, date, PostgresAccountManager.LoggedInUserId);

            Console.WriteLine($"{date:dd MMM yyyy} {name} {amount}"); // Remove and return transactions list instead
        }

        return transactions;
    }
}
