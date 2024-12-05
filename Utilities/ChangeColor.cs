namespace Individuell_Uppgift.Utilities;

public class ChangeColor
{
    public static void TextColorRed(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(message);
        Console.ResetColor();
    }

    public static void TextColorGreen(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(message);
        Console.ResetColor();
    }
}
