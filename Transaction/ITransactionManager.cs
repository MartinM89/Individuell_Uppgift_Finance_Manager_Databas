public interface ITransactionManager
{
    void AddTransaction(Transaction transaction);
    int DeleteTransaction(Guid userGuid, int transactionToDelete);
    decimal GetBalance(Guid userGuid);
    List<Transaction> GetAllTransactions(Guid userGuid);
    List<Transaction> GetTransactionsByDay(Guid userGuid, int dayOfMonth, bool isIncome);
    List<Transaction> GetTransactionsByWeek(Guid userGuid, int weekNumber, bool isIncome);
    List<Transaction> GetTransactionsByMonth(Guid userGuid, int monthNumber, bool isIncome);
    List<Transaction> GetTransactionsByYear(Guid userGuid, int yearNumber, bool isIncome);
    void TransferFunds(Transaction transaction);
}
