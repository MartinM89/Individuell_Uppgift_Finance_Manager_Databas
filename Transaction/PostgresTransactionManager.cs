using Npgsql;

public class PostgresTransactionManager : ITransactionManager
{
    private readonly NpgsqlConnection connection;

    public PostgresTransactionManager(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public void AddTransaction(Transaction transaction)
    {
        string insertTransactionSql = "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @amount, @user_id)";

        try
        {
            using NpgsqlCommand insertTransactionCmd = new(insertTransactionSql, connection);
            insertTransactionCmd.Parameters.AddWithValue("@name", transaction.Name!);
            insertTransactionCmd.Parameters.AddWithValue("@amount", transaction.Amount);
            insertTransactionCmd.Parameters.AddWithValue("@user_id", transaction.UserId);

            insertTransactionCmd.ExecuteNonQuery();
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to add a transaction to database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to add a transaction.", ex);
        }
    }

    public int DeleteTransaction(int transactionToDelete)
    {
        string deleteTransactionSql = "DELETE FROM transactions WHERE user_id = @user_id AND id = @id";

        try
        {
            using NpgsqlCommand deleteTransactionCmd = new(deleteTransactionSql, connection);
            deleteTransactionCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.GetLoggedInUserId());
            deleteTransactionCmd.Parameters.AddWithValue("@id", transactionToDelete);
            return deleteTransactionCmd.ExecuteNonQuery();
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to delete a transaction from database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to delete a transaction.", ex);
        }
    }

    public decimal GetBalance()
    {
        string getBalanceSql = "SELECT amount FROM transactions WHERE user_id = @user_id";

        try
        {
            using NpgsqlCommand getBalanceCmd = new(getBalanceSql, connection);
            getBalanceCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.GetLoggedInUserId());

            using NpgsqlDataReader reader = getBalanceCmd.ExecuteReader();

            decimal totalBalance = 0;

            while (reader.Read())
            {
                totalBalance += reader.GetDecimal(0);
            }

            return totalBalance;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to get balance from database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to get balance.", ex);
        }
    }

    public List<Transaction> GetAllTransactions()
    {
        string getAllTransactionsSql = "SELECT * FROM transactions WHERE user_id = @user_id";

        try
        {
            using NpgsqlCommand getAllTransactionsCmd = new(getAllTransactionsSql, connection);
            getAllTransactionsCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.GetLoggedInUserId());

            List<Transaction> transactions = [];

            using (NpgsqlDataReader reader = getAllTransactionsCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), PostgresAccountManager.GetLoggedInUserId());

                    transactions.Add(transaction);
                }
            }

            return transactions;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to list all transactions from database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to list all transactions.", ex);
        }
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

        try
        {
            using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByDaySql, connection);
            getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.GetLoggedInUserId());
            getTransactionsByDayCmd.Parameters.AddWithValue("@dayOfMonth", dayOfMonth);

            List<Transaction> transactions = [];

            using (NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), PostgresAccountManager.GetLoggedInUserId());

                    transactions.Add(transaction);
                }
            }

            return transactions;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to get transaction filtered by (DAY) from database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to get transaction filtered by (DAY).", ex);
        }
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

        try
        {
            using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByWeekSql, connection);
            getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.GetLoggedInUserId());
            getTransactionsByDayCmd.Parameters.AddWithValue("@weekNumber", weekNumber);

            List<Transaction> transactions = [];

            using (NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), PostgresAccountManager.GetLoggedInUserId());

                    transactions.Add(transaction);
                }
            }

            return transactions;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to get transaction filtered by (WEEK) from database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to get transaction filtered by (WEEK).", ex);
        }
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

        try
        {
            using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByMonthSql, connection);
            getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.GetLoggedInUserId());
            getTransactionsByDayCmd.Parameters.AddWithValue("@month", month);

            List<Transaction> transactions = [];

            using (NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), PostgresAccountManager.GetLoggedInUserId());

                    transactions.Add(transaction);
                }
            }

            return transactions;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to get transaction filtered by (MONTH) from database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to get transaction filtered by (MONTH).", ex);
        }
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

        try
        {
            using NpgsqlCommand getTransactionsByDayCmd = new(getTransactionsByYearSql, connection);
            getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", PostgresAccountManager.GetLoggedInUserId());
            getTransactionsByDayCmd.Parameters.AddWithValue("@year", year);

            List<Transaction> transactions = [];

            using (NpgsqlDataReader reader = getTransactionsByDayCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), PostgresAccountManager.GetLoggedInUserId());

                    transactions.Add(transaction);
                }
            }

            return transactions;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to get transaction filtered by (YEAR) from database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to get transaction filtered by (YEAR).", ex);
        }
    }
}
