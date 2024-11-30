public interface ITransactionManager
{
    void SaveTransaction(Transaction transaction);
    void DeleteTransaction(int deleteTransaction);
    void GetBalance();
    void GetAllTransactions();
    Transaction GetTransactionsByDay(int dayOfMonth, char transactionType);
    Transaction GetTransactionsByWeek(int weekNumber, char transactionType);
    Transaction GetTransactionsByMonth(int month, char transactionType);
    Transaction GetTransactionsByYear(int year, char transactionType);
}
