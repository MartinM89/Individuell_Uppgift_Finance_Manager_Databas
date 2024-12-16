using Npgsql;
using Npgsql.Replication.PgOutput.Messages;

public class TransactionExample
{
    public static void Execute()
    {
        using var connection = new NpgsqlConnection("connectionString");
        connection.Open();

        // Starta en transaktion - om något går fel så ångras alla ändringar
        using var transaction = connection.BeginTransaction();

        try
        {
            // Först lägger vi till husdjuret
            var petSql = """
                INSERT INTO pets (name, type, favoriteFood)
                VALUES (@name, @type, @food)
                RETURNING id
                """; // RETURNING id ger oss det nya ID:t

            using var petCmd = new NpgsqlCommand(petSql, connection, transaction);
            petCmd.Parameters.AddWithValue("name", "Luna");
            petCmd.Parameters.AddWithValue("type", "cat");
            petCmd.Parameters.AddWithValue("food", "tuna");

            // ExecuteScalar används när vi förväntar oss ett enda värde tillbaka
            var newPetId = (int)petCmd.ExecuteScalar()!;

            // Sen lägger vi till kunden
            var customerSql = """
                INSERT INTO customers (name, email, petId)
                VALUES (@name, @email, @petId)
                """;

            using var customerCmd = new NpgsqlCommand(customerSql, connection, transaction);
            customerCmd.Parameters.AddWithValue("name", "Maria");
            customerCmd.Parameters.AddWithValue("email", "maria@mail.com");
            customerCmd.Parameters.AddWithValue("petId", newPetId);

            // ExecuteNonQuery används när vi inte förväntar oss något resultat
            customerCmd.ExecuteNonQuery();

            // Om vi kommer hit har allt gått bra och vi kan spara ändringarna
            transaction.Commit();
            Console.WriteLine("Ny kund och husdjur har lagts till!");
        }
        catch (Exception ex)
        {
            // Om något går fel så ångrar vi alla ändringar
            transaction.Rollback();
            Console.WriteLine($"Något gick fel: {ex.Message}");
        }
    }

    public void SendTransactionToOtherUser(Transaction transaction)
    {
        using NpgsqlConnection connection = new("connectionString");
        connection.Open();

        using NpgsqlTransaction sqlTransaction = connection.BeginTransaction();

        string SendTransactionToOtherUserSql =
            "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @send_amount, @sender_user_id);"
            + "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @recieve_amount, @reciever_user_id);";

        try
        {
            using NpgsqlCommand SendTransactionToOtherUserCmd = new(SendTransactionToOtherUserSql, connection, sqlTransaction);
            SendTransactionToOtherUserCmd.Parameters.AddWithValue("@name", transaction.Name!);
            SendTransactionToOtherUserCmd.Parameters.AddWithValue("@send_amount", -transaction.Amount);
            SendTransactionToOtherUserCmd.Parameters.AddWithValue("@recieve_amount", transaction.Amount);
            SendTransactionToOtherUserCmd.Parameters.AddWithValue("@sender_user_id", PostgresAccountManager.GetLoggedInUserId());
            SendTransactionToOtherUserCmd.Parameters.AddWithValue("@reciever_user_id", transaction.UserId);

            SendTransactionToOtherUserCmd.ExecuteNonQuery();
            sqlTransaction.Commit();
        }
        catch (NpgsqlException ex)
        {
            sqlTransaction.Rollback();
            throw new Exception($"PostgreSQL error: {ex.Message}\nAn error occured while attempting to send a transaction to another user to database.", ex);
        }
        catch (Exception ex)
        {
            sqlTransaction.Rollback();
            throw new Exception($"Error: {ex.Message}\nAn error occured while attempting to send a transaction to another user.", ex);
        }
    }

    public void SendTransactionToOtherUserTwo(Transaction transaction)
    {
        using NpgsqlConnection connection = new("connectionString");
        connection.Open();

        string SendTransactionToOtherUserSql =
            "BEGIN;"
            + "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @send_amount, @sender_user_id);"
            + "INSERT INTO transactions (name, amount, user_id) VALUES (@name, @recieve_amount, @reciever_user_id);"
            + "COMMIT;";

        try
        {
            using NpgsqlCommand SendTransactionToOtherUserCmd = new(SendTransactionToOtherUserSql, connection);
            SendTransactionToOtherUserCmd.Parameters.AddWithValue("@name", transaction.Name!);
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
