namespace Individuell_Uppgift.Utilities;

public class HideCursor
{
    public static string Execute()
    {
        string input = "";

        do
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            input += keyInfo.KeyChar;
            Console.Write(" ");
        } while (input.Length.Equals(0));

        return input;
    }
}


// namespace Individuell_Uppgift.Utilities;

// public class HideCursor
// {
//     public static string Execute()
//     {
//         string input = "";
//         ConsoleKey key;

//         do
//         {
//             ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
//             key = keyInfo.Key;

//             if (key == ConsoleKey.Backspace && input.Length > 0)
//             {
//                 input = input[0..^1];
//                 Console.Write("\b \b");
//             }
//             else if (!char.IsControl(keyInfo.KeyChar))
//             {
//                 input += keyInfo.KeyChar;
//                 Console.Write(" ");
//             }
//         } while (input.Length.Equals(0));

//         return input;
//     }
// }
