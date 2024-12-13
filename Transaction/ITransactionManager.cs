public interface ITransactionManager
{
    Task AddTransaction(Transaction transaction);
    Task<int> DeleteTransaction(int transactionToDelete);
    Task<decimal> GetBalance();
    Task<List<Transaction>> GetAllTransactions();
    Task<List<Transaction>> GetTransactionsByDay(int dayOfMonth, bool isCredit);
    Task<List<Transaction>> GetTransactionsByWeek(int weekNumber, bool isCredit);
    Task<List<Transaction>> GetTransactionsByMonth(int monthNumber, bool isCredit);
    Task<List<Transaction>> GetTransactionsByYear(int yearNumber, bool isCredit);
}
