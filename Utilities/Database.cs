using DotNetEnv;
using Npgsql;

public class Database
{
    public static string GetConnectionString()
    {
        Env.Load();

        return Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? throw new Exception("Can't access connection string to connect to database");
    }

    public static async Task Initialize(NpgsqlConnection connection)
    {
        #region Database tables, functions and triggers query
        string createTablesSql = """
                CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

                CREATE TABLE IF NOT EXISTS users (
                id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
                username VARCHAR(25) UNIQUE NOT NULL,
                password_hash VARCHAR(100) NOT NULL,
                password_salt VARCHAR NOT NULL
                );

                CREATE TABLE IF NOT EXISTS transactions (
                internal_id SERIAL PRIMARY KEY,
                name VARCHAR NOT NULL,
                amount DECIMAL(10, 2) NOT NULL,
                date DATE DEFAULT NOW() NOT NULL,
                user_id UUID NOT NULL,
                FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
            );

                CREATE OR REPLACE VIEW user_transactions_view AS
                SELECT
                    ROW_NUMBER() OVER (PARTITION BY user_id ORDER BY date) AS transaction_id,
                    internal_id,
                    name,
                    amount,
                    date,
                    user_id
                FROM transactions;
            """;
        #endregion

        try
        {
            using NpgsqlCommand createTablesCmd = new(createTablesSql, connection);
            await createTablesCmd.ExecuteNonQueryAsync();
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to create tables, functions and/or triggers in database.", ex);
        }
    }
}
