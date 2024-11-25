public interface ITransactionManager
{
    // void AddTransaction();
    void SaveTransaction(Transaction transaction);
    void DeleteTransaction(int id);
    void GetBalance();
    List<Transaction> GetTransactions();
    void PrintTransactions();
    void LoadTransactions(List<Transaction> loadTransactions);
}
