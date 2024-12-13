public class LongestTransationLength
{
    public static int Execute(List<Transaction> transactions)
    {
        int longestWord = 0;
        foreach (Transaction transaction in transactions)
        {
            if (transaction.Name == null)
            {
                return 0;
            }

            if (transaction.Name.Length > longestWord)
            {
                longestWord = transaction.Name.Length;
            }
        }
        return longestWord;
    }
}
