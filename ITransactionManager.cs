public interface ITransactionManager
{
    // void AddTransaction();
    void SaveTransaction(Transaction transaction);
    void DeleteTransaction(int deleteTransaction);
    void GetBalance();
    void GetTransactions();
    void PrintTransactions();
    void LoadTransactions(List<Transaction> loadTransactions);
}
