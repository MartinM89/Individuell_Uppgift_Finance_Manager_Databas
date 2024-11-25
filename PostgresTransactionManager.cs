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

    // public void AddTransaction(Transaction transaction)
    // {
    //     string capitalizedTransactionName;

    //     while (true)
    //     {
    //         bool onlyLettersOrWhiteSpace = true;

    //         Console.Clear();
    //         Console.Write("Enter name: ");
    //         string transactionName = Console.ReadLine()!.ToLower();

    //         if (string.IsNullOrEmpty(transactionName))
    //         {
    //             return;
    //         }

    //         foreach (char c in transactionName)
    //         {
    //             if (!char.IsLetter(c) && !char.IsWhiteSpace(c)) // Invert and continue?
    //             {
    //                 onlyLettersOrWhiteSpace = false;
    //             }
    //         }

    //         if (transactionName.Length! < 3 || transactionName.Length! > 20 || !onlyLettersOrWhiteSpace)
    //         {
    //             Console.Clear();
    //             Console.WriteLine("Invalid Input. Name must be at least 3 letters long, at most 20 letters long and not contain numbers or symbols.");
    //             PressKeyToContinue.Execute();
    //             continue;
    //         }

    //         capitalizedTransactionName = char.ToUpper(transactionName[0]) + transactionName.Substring(1);
    //         break;
    //     }

    //     decimal transactionValue;

    //     while (true)
    //     {
    //         Console.Clear();
    //         Console.Write("Enter amount: ");
    //         string transactionValueString = Console.ReadLine()!;

    //         if (string.IsNullOrEmpty(transactionValueString))
    //         {
    //             return;
    //         }

    //         if (transactionValueString.Length > 10 || !decimal.TryParse(transactionValueString, out transactionValue))
    //         {
    //             Console.Clear();
    //             Console.WriteLine("Invalid Input. Amount must be only numbers and not exceed 10 numbers long.");
    //             PressKeyToContinue.Execute();
    //             continue;
    //         }

    //         if (transactionValue.Equals(0))
    //         {
    //             Console.Clear();
    //             Console.WriteLine("Invalid Input. Amount can't be 0.");
    //             PressKeyToContinue.Execute();
    //             continue;
    //         }

    //         break;
    //     }

    //     var userId = Guid.NewGuid();

    //     SaveTransaction(capitalizedTransactionName, transactionValue, userId);
    // }

    public void SaveTransaction(Transaction transaction)
    {
        var insertTransactionSql = "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @amount, @user_id)";
        using var insertTransactionCmd = new NpgsqlCommand(insertTransactionSql, connection);
        insertTransactionCmd.Parameters.AddWithValue("@name", transaction.Name!);
        insertTransactionCmd.Parameters.AddWithValue("@amount", transaction.Amount);
        insertTransactionCmd.Parameters.AddWithValue("@user_id", transaction.Id);

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
