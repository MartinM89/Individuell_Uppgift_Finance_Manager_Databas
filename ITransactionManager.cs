public interface ITransactionManager
{
    void SaveTransaction(Transaction transaction);
    void DeleteTransaction(int transactionToDelete);
    void GetBalance();
    void GetAllTransactions();
    Transaction GetTransactionsByDay(int dayOfMonth, char transactionType);
    Transaction GetTransactionsByWeek(int weekNumber, char transactionType);
    Transaction GetTransactionsByMonth(int monthNumber, char transactionType);
    Transaction GetTransactionsByYear(int yearNumber, char transactionType);
}
