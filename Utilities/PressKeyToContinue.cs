public class PressKeyToContinue
{
    public static void Execute()
    {
        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey(intercept: true);
    }
}
