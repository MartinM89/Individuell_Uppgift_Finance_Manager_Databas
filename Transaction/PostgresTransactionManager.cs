using Npgsql;

public class PostgresTransactionManager : ITransactionManager
{
    private readonly NpgsqlConnection Connection;

    public PostgresTransactionManager(NpgsqlConnection connection)
    {
        Connection = connection;
    }

    public void AddTransaction(Transaction transaction)
    {
        string insertTransactionSql = "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @amount, @user_id)";

        try
        {
            using NpgsqlCommand insertTransactionCmd = new(insertTransactionSql, Connection);
            insertTransactionCmd.Parameters.AddWithValue("@name", transaction.Name!);
            insertTransactionCmd.Parameters.AddWithValue("@amount", transaction.Amount);
            insertTransactionCmd.Parameters.AddWithValue("@user_id", transaction.UserId);

            insertTransactionCmd.ExecuteNonQuery();
        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine($"PostgreSQL error: {ex.Message}");
            throw new Exception("An error occured while attempting to add a transaction to database.", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw new Exception("An error occured while attempting to add a transaction.", ex);
        }
    }

    public int DeleteTransaction(int transactionToDelete)
    {
        string deleteTransactionSql = "DELETE FROM transactions WHERE user_id = @user_id AND id = @id";
        using NpgsqlCommand deleteTransactionCmd = new(deleteTransactionSql, Connection);
        deleteTransactionCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);
        deleteTransactionCmd.Parameters.AddWithValue("@id", transactionToDelete);

        return deleteTransactionCmd.ExecuteNonQuery();
    }

    public decimal GetBalance()
    {
        string getBalanceSql = "SELECT amount FROM transactions WHERE user_id = @user_id";
        using NpgsqlCommand getBalanceCmd = new(getBalanceSql, Connection);
        getBalanceCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);

        using NpgsqlDataReader reader = getBalanceCmd.ExecuteReader();

        decimal totalBalance = 0;

        while (reader.Read())
        {
            totalBalance += reader.GetDecimal(0);
        }

        return totalBalance;
    }

    public List<Transaction> GetAllTransactions()
    {
        string getAllTransactionsSql = "SELECT * FROM transactions WHERE user_id = @user_id";
        using NpgsqlCommand getAllTransactionsCmd = new(getAllTransactionsSql, Connection);
        getAllTransactionsCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);

        List<Transaction> transactions = [];

        using (NpgsqlDataReader reader = getAllTransactionsCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), PostgresAccountManager.LoggedInUserId);

                transactions.Add(transaction);
            }
        }

        return transactions;
    }

    public List<Transaction> GetTransactionsByDay(int dayOfMonth, bool isCredit)
    {
        string getTransactionsByDaySql = isCredit
            ? """
                SELECT * FROM transactions
                WHERE user_id = @user_id
                AND @dayOfMonth IS NOT NULL AND EXTRACT(DAY FROM date) = @dayOfMonth
                AND amount > 0
                """
            : """
                SELECT * FROM transactions
                WHERE user_id = @user_id
                AND @dayOfMonth IS NOT NULL AND EXTRACT(DAY FROM date) = @dayOfMonth
                AND amount < 0
                """;

        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByDaySql, Connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@dayOfMonth", dayOfMonth);

        List<Transaction> transactions = [];

        using (NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), PostgresAccountManager.LoggedInUserId);

                transactions.Add(transaction);
            }
        }

        return transactions;
    }

    public List<Transaction> GetTransactionsByWeek(int weekNumber, bool isCredit)
    {
        string getTransactionsByWeekSql = isCredit
            ? """
                SELECT * FROM transactions
                WHERE user_id = @user_id
                AND @weekNumber IS NOT NULL AND EXTRACT(WEEK FROM date) = @weekNumber
                AND amount > 0
                """
            : """
                SELECT * FROM transactions
                WHERE user_id = @user_id
                AND @weekNumber IS NOT NULL AND EXTRACT(WEEK FROM date) = @weekNumber
                AND amount < 0
                """;
        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByWeekSql, Connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@weekNumber", weekNumber);

        List<Transaction> transactions = [];

        using (NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), PostgresAccountManager.LoggedInUserId);

                transactions.Add(transaction);
            }
        }

        return transactions;
    }

    public List<Transaction> GetTransactionsByMonth(int month, bool isCredit)
    {
        string getTransactionsByMonthSql = isCredit
            ? """
                SELECT * FROM transactions
                WHERE user_id = @user_id
                AND @month IS NOT NULL AND EXTRACT(MONTH FROM date) = @month
                AND amount > 0
                """
            : """
                SELECT * FROM transactions
                WHERE user_id = @user_id
                AND @month IS NOT NULL AND EXTRACT(MONTH FROM date) = @month
                AND amount < 0
                """;
        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByMonthSql, Connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@month", month);

        List<Transaction> transactions = [];

        using (NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), PostgresAccountManager.LoggedInUserId);

                transactions.Add(transaction);
            }
        }

        return transactions;
    }

    public List<Transaction> GetTransactionsByYear(int year, bool isCredit)
    {
        string getTransactionsByYearSql = isCredit
            ? """
                SELECT * FROM transactions
                WHERE user_id = @user_id
                AND @year IS NOT NULL AND EXTRACT(YEAR FROM date) = @year
                AND amount > 0
                """
            : """
                SELECT * FROM transactions
                WHERE user_id = @user_id
                AND @year IS NOT NULL AND EXTRACT(YEAR FROM date) = @year
                AND amount < 0
                """;
        using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByYearSql, Connection);
        getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.LoggedInUserId);
        getTransactionsByDayCmd.Parameters.AddWithValue("@year", year);

        List<Transaction> transactions = [];

        using (NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), PostgresAccountManager.LoggedInUserId);

                transactions.Add(transaction);
            }
        }

        return transactions;
    }
}
