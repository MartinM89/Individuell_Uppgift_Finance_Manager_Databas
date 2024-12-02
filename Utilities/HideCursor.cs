public class HideCursor
{
    public static string Execute(string input)
    {
        Console.CursorVisible = false;
        ConsoleKey key;

        do
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input[0..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                input += keyInfo.KeyChar;
                Console.Write(" ");
            }
        } while (input.Length.Equals(0));

        Console.CursorVisible = true;
        return input;
    }
}
