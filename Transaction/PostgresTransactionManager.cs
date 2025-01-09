using Npgsql;

public class PostgresTransactionManager : ITransactionManager
{
    private readonly NpgsqlConnection connection;

    public PostgresTransactionManager(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public async Task AddTransaction(Transaction transaction)
    {
        if (transaction.Name == null)
        {
            throw new Exception("Error: Could not find transaction name while attempting to add transaction.");
        }

        string insertTransactionSql = "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @amount, @user_id)";

        try
        {
            using NpgsqlCommand insertTransactionCmd = new(insertTransactionSql, connection);
            insertTransactionCmd.Parameters.AddWithValue("@name", transaction.Name);
            insertTransactionCmd.Parameters.AddWithValue("@amount", transaction.Amount);
            insertTransactionCmd.Parameters.AddWithValue("@user_id", transaction.UserId);

            await insertTransactionCmd.ExecuteNonQueryAsync();
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to add a transaction to database.");
        }
    }

    public async Task<int> DeleteTransaction(Guid userGuid, int transactionToDelete)
    {
        string deleteTransactionSql = """
                WITH to_delete AS (
                    SELECT internal_id
                    FROM user_transactions_view
                    WHERE user_id = @user_id AND transaction_id = @transaction_id
                )

                DELETE FROM transactions
                WHERE internal_id = (SELECT internal_id FROM to_delete);
            """;

        try
        {
            using NpgsqlCommand cmd = new(deleteTransactionSql, connection);
            cmd.Parameters.AddWithValue("@user_id", userGuid);
            cmd.Parameters.AddWithValue("@transaction_id", transactionToDelete);

            return await cmd.ExecuteNonQueryAsync();
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occurred while attempting to delete a transaction from the database.");
        }
    }

    public async Task<decimal> GetBalance(Guid userGuid)
    {
        userGuid = userGuid.Equals(Guid.Empty) ? PostgresAccountManager.GetLoggedInUserId() : userGuid;

        string getBalanceSql = "SELECT amount FROM transactions WHERE user_id = @user_id";

        try
        {
            using NpgsqlCommand getBalanceCmd = new(getBalanceSql, connection);
            getBalanceCmd.Parameters.AddWithValue("@user_id", userGuid);

            using NpgsqlDataReader reader = await getBalanceCmd.ExecuteReaderAsync();

            decimal totalBalance = 0;

            while (await reader.ReadAsync())
            {
                totalBalance += reader.GetDecimal(0);
            }

            return totalBalance;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to get balance from database.");
        }
    }

    public async Task<List<Transaction>> GetAllTransactions(Guid userGuid)
    {
        userGuid = userGuid.Equals(Guid.Empty) ? PostgresAccountManager.GetLoggedInUserId() : userGuid;

        string getAllTransactionsSql = "SELECT transaction_id, name, amount, date FROM user_transactions_view WHERE user_id = @user_id";

        try
        {
            using NpgsqlCommand getAllTransactionsCmd = new(getAllTransactionsSql, connection);
            getAllTransactionsCmd.Parameters.AddWithValue("@user_id", userGuid);

            List<Transaction> transactions = [];

            using (NpgsqlDataReader reader = await getAllTransactionsCmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), PostgresAccountManager.GetLoggedInUserId());

                    transactions.Add(transaction);
                }
            }

            return transactions;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to list all transactions from database.");
        }
    }

    public async Task<List<Transaction>> GetTransactionsByTime(Guid userGuid, int timeValue, string timeInterval, string incomeOrExpense)
    {
        userGuid = userGuid.Equals(Guid.Empty) ? PostgresAccountManager.GetLoggedInUserId() : userGuid;

        string getTransactionsByTimeSql = """
            SELECT transaction_id, name, amount, date
            FROM user_transactions_view
            WHERE user_id = @user_id
            AND @timeValue IS NOT NULL AND EXTRACT(EXTRACT_TIME FROM date) = @timeValue
            AND amount INCOME_OR_EXPENSE 0
            """;

        getTransactionsByTimeSql = getTransactionsByTimeSql.Replace("EXTRACT_TIME", timeInterval);
        getTransactionsByTimeSql = getTransactionsByTimeSql.Replace("INCOME_OR_EXPENSE", incomeOrExpense);

        try
        {
            using NpgsqlCommand getTransactionsByTimeCmd = new(getTransactionsByTimeSql, connection);
            getTransactionsByTimeCmd.Parameters.AddWithValue("@user_id", userGuid);
            getTransactionsByTimeCmd.Parameters.AddWithValue("@timeValue", timeValue);

            List<Transaction> transactions = new();

            using (NpgsqlDataReader reader = await getTransactionsByTimeCmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Transaction transaction = new(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetDateTime(3), userGuid);

                    transactions.Add(transaction);
                }
            }

            return transactions;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occurred while attempting to get transactions filtered by ({timeInterval}) from the database.");
        }
    }

    public async Task TransferFunds(Transaction transaction)
    {
        if (transaction.Name == null)
        {
            throw new Exception("Error: Could not find transaction name while attempting to transfer funds.");
        }

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

            await SendTransactionToOtherUserCmd.ExecuteNonQueryAsync();
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to send a transaction to another user to database.");
        }
    }

    public async Task ShowBalancePerUser() // Fix this
    {
        string showBalancePerUserSql = """
            SELECT u.username, COALESCE(SUM(t.amount), 0) AS total_balance
            FROM users u
            LEFT JOIN transactions t ON u.id = t.user_id
            GROUP BY u.id;
            """;

        try
        {
            Console.Clear();

            using NpgsqlCommand showBalancePerUserCmd = new(showBalancePerUserSql, connection);

            using NpgsqlDataReader reader = await showBalancePerUserCmd.ExecuteReaderAsync();

            Console.WriteLine("| User   | Total balance |");
            Console.WriteLine(" --------+---------------");

            while (await reader.ReadAsync())
            {
                if (reader.GetString(0).Equals("Admin"))
                {
                    continue;
                }

                Console.WriteLine($"| {reader.GetString(0)} | {reader.GetDecimal(1), 13} |");
            }

            Console.WriteLine(" ------------------------");
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to show balance per user from database.");
        }
    }
}
