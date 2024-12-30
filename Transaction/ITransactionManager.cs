public interface ITransactionManager
{
    Task AddTransaction(Transaction transaction);
    Task<int> DeleteTransaction(Guid userGuid, int transactionToDelete);
    Task<decimal> GetBalance(Guid userGuid);
    Task<List<Transaction>> GetAllTransactions(Guid userGuid);
    Task<List<Transaction>> GetTransactionsByDay(Guid userGuid, int dayOfMonth, bool isIncome);
    Task<List<Transaction>> GetTransactionsByWeek(Guid userGuid, int weekNumber, bool isIncome);
    Task<List<Transaction>> GetTransactionsByMonth(Guid userGuid, int monthNumber, bool isIncome);
    Task<List<Transaction>> GetTransactionsByYear(Guid userGuid, int yearNumber, bool isIncome);
    Task TransferFunds(Transaction transaction);
}
