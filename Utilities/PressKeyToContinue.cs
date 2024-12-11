public class PressKeyToContinue
{
    public static void Execute()
    {
        Console.CursorVisible = false;
        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey(intercept: true);
        Console.CursorVisible = true;
    }
}
