public class TransactionMenu
{
    public static void Execute()
    {
        Console.Clear();

        Console.WriteLine("What do you wish to do?\n");
        ChangeColor.TextColorGreen("[A]");
        Console.WriteLine("dd Transaction");
        ChangeColor.TextColorGreen("[D]");
        Console.WriteLine("elete Transaction");
        Console.Write("Check ");
        ChangeColor.TextColorGreen("[B]");
        Console.WriteLine("alance");
        Console.Write("Check ");
        ChangeColor.TextColorGreen("[I]");
        Console.WriteLine("ncome Summary");
        Console.Write("Check ");
        ChangeColor.TextColorGreen("[E]");
        Console.WriteLine("xpense Summary");
        ChangeColor.TextColorGreen("[L]");
        Console.WriteLine("og Out");
        ChangeColor.TextColorGreen("[H]");
        Console.WriteLine("elp Page");

        // Console.Write("\nEnter choice: ");
    }
}
