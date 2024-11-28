public interface ITransactionManager
{
    // void AddTransaction();
    void SaveTransaction(Transaction transaction);
    void DeleteTransaction(Transaction transaction, int deleteTransaction);
    void GetBalance();
    List<Transaction> GetTransactions();
    void PrintTransactions();
    void LoadTransactions(List<Transaction> loadTransactions);
}
