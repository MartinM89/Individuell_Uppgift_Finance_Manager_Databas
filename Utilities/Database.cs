using DotNetEnv;
using Npgsql;

public class Database
{
    public static string GetConnectionString()
    {
        Env.Load();

        return Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? throw new Exception("Can't access connection string to connect to database");
    }

    public static void Initialize(NpgsqlConnection connection)
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

        try
        {
            using NpgsqlCommand createTablesCmd = new(createTablesSql, connection);
            createTablesCmd.ExecuteNonQuery();
        }
        catch (NpgsqlException ex)
        {
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to create tables, functions and/or triggers in database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to create tables, functions and/or triggers.", ex);
        }
    }
}
