public interface ITransactionManager
{
    void AddTransaction(Transaction transaction);
    int DeleteTransaction(int transactionToDelete);
    decimal GetBalance();
    List<Transaction> GetAllTransactions();
    List<Transaction> GetTransactionsByDay(int dayOfMonth, bool isCredit);
    List<Transaction> GetTransactionsByWeek(int weekNumber, bool isCredit);
    List<Transaction> GetTransactionsByMonth(int monthNumber, bool isCredit);
    List<Transaction> GetTransactionsByYear(int yearNumber, bool isCredit);
    void SendTransactionToOtherUser(Transaction transaction);
}
