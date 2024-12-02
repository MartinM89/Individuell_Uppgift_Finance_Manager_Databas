public class HidePassword
{
    public static string Execute(string password)
    {
        Console.CursorVisible = false;
        ConsoleKey key;

        do
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                password += keyInfo.KeyChar;
                Console.Write("●");
            }
        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;
        return password;
    }
}
