public interface ITransactionManager
{
    Task AddTransaction(Transaction transaction);
    Task<int> DeleteTransaction(int transactionToDelete);
    Task<decimal> GetBalance();
    Task<List<Transaction>> GetAllTransactions();
    Task<List<Transaction>> GetTransactionsByDay(int dayOfMonth, char transactionType);
    Task<List<Transaction>> GetTransactionsByWeek(int weekNumber, char transactionType);
    Task<List<Transaction>> GetTransactionsByMonth(int monthNumber, char transactionType);
    Task<List<Transaction>> GetTransactionsByYear(int yearNumber, char transactionType);
}
