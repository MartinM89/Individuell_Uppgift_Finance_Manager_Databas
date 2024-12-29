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
            insertTransactionCmd.Parameters.AddWithValue("@name", transaction.Name);
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

    public decimal GetBalance(Guid userGuid)
    {
        userGuid = userGuid.Equals(Guid.Empty) ? PostgresAccountManager.GetLoggedInUserId() : userGuid;

        string getBalanceSql = "SELECT amount FROM transactions WHERE user_id = @user_id";

        try
        {
            using NpgsqlCommand getBalanceCmd = new(getBalanceSql, connection);
            getBalanceCmd.Parameters.AddWithValue("@user_id", userGuid);

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

    public List<Transaction> GetAllTransactions(Guid userGuid)
    {
        userGuid = userGuid.Equals(Guid.Empty) ? PostgresAccountManager.GetLoggedInUserId() : userGuid;

        string getAllTransactionsSql = "SELECT * FROM transactions WHERE user_id = @user_id";

        try
        {
            using NpgsqlCommand getAllTransactionsCmd = new(getAllTransactionsSql, connection);
            getAllTransactionsCmd.Parameters.AddWithValue("@user_id", userGuid);

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

    public List<Transaction> GetTransactionsByDay(Guid userGuid, int dayOfMonth, bool isIncome)
    {
        userGuid = userGuid.Equals(Guid.Empty) ? PostgresAccountManager.GetLoggedInUserId() : userGuid;

        string getTransactionsByDaySql = isIncome
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
            getTransactionsByDayCmd.Parameters.AddWithValue("@user_id", userGuid);
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

    public List<Transaction> GetTransactionsByWeek(Guid userGuid, int weekNumber, bool isIncome)
    {
        userGuid = userGuid.Equals(Guid.Empty) ? PostgresAccountManager.GetLoggedInUserId() : userGuid;

        string getTransactionsByWeekSql = isIncome
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

    public List<Transaction> GetTransactionsByMonth(Guid userGuid, int month, bool isIncome)
    {
        userGuid = userGuid.Equals(Guid.Empty) ? PostgresAccountManager.GetLoggedInUserId() : userGuid;

        string getTransactionsByMonthSql = isIncome
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

    public List<Transaction> GetTransactionsByYear(Guid userGuid, int year, bool isIncome)
    {
        userGuid = userGuid.Equals(Guid.Empty) ? PostgresAccountManager.GetLoggedInUserId() : userGuid;

        string getTransactionsByYearSql = isIncome
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

    public void TransferFunds(Transaction transaction)
    {
        string SendTransactionToOtherUserSql =
            "BEGIN;"
            + "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @send_amount, @sender_user_id);"
            + "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @recieve_amount, @reciever_user_id);"
            + "COMMIT;";

        try
        {
            using NpgsqlCommand SendTransactionToOtherUserCmd = new(SendTransactionToOtherUserSql, connection);
            SendTransactionToOtherUserCmd.Parameters.AddWithValue("@name", transaction.Name);
            SendTransactionToOtherUserCmd.Parameters.AddWithValue("@send_amount", -transaction.Amount);
            SendTransactionToOtherUserCmd.Parameters.AddWithValue("@recieve_amount", transaction.Amount);
            SendTransactionToOtherUserCmd.Parameters.AddWithValue("@sender_user_id", PostgresAccountManager.GetLoggedInUserId());
            SendTransactionToOtherUserCmd.Parameters.AddWithValue("@reciever_user_id", transaction.UserId);

            SendTransactionToOtherUserCmd.ExecuteNonQuery();
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to send a transaction to another user to database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to send a transaction to another user.", ex);
        }
    }
}


// SHOWS BALANCE PER USER

// CREATE VIEW user_balances AS
// SELECT
//     users.id AS user_id,
//     users.username,
//     COALESCE(SUM(transactions.amount), 0) AS total_balance
// FROM
//     users
// LEFT JOIN
//     transactions ON users.id = transactions.user_id
// GROUP BY
//     users.id, users.username;
