public interface ITransactionManager
{
    void AddTransaction(Transaction transaction);
    void DeleteTransaction(int transactionToDelete);
    decimal GetBalance();
    List<Transaction> GetAllTransactions();
    List<Transaction> GetTransactionsByDay(int dayOfMonth, char transactionType);
    List<Transaction> GetTransactionsByWeek(int weekNumber, char transactionType);
    List<Transaction> GetTransactionsByMonth(int monthNumber, char transactionType);
    List<Transaction> GetTransactionsByYear(int yearNumber, char transactionType);
}
