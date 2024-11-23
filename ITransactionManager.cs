public interface ITransactionManager
{
    void AddTransaction();
    void SaveTransaction(string name, decimal amount, Guid userId);
    void DeleteTransaction(int id);
    void GetBalance();
    List<Transaction> GetTransactions();
    void PrintTransactions();
    void LoadTransactions(List<Transaction> loadTransactions);
}
