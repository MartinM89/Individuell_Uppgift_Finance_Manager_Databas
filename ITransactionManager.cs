public interface ITransactionManager
{
    void SaveTransaction(Transaction transaction);
    void DeleteTransaction(int deleteTransaction);
    void GetBalance();
    void GetAllTransactions();
    Transaction GetTransactionsByDay(int day);
}
