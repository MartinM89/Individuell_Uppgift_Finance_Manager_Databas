public interface ITransactionManager
{
    Task AddTransaction(Transaction transaction);
    Task<int> DeleteTransaction(Guid userGuid, int transactionToDelete);
    Task<decimal> GetBalance(Guid userGuid);
    Task<List<Transaction>> GetAllTransactions(Guid userGuid);
    Task<List<Transaction>> GetTransactionsByTime(Guid userGuid, int timeValue, string timeInterval, string incomeOrExpense);
    Task TransferFunds(Transaction transaction);
    Task ShowBalancePerUser();
}
