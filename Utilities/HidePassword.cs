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
                // char random = RandomCharGenerator.Execute();
                Console.Write(" ");
            }
        } while (key != ConsoleKey.Enter);

        return password;
    }
}

public class RandomCharGenerator
{
    public static char Execute()
    {
        Random random = new Random();
        int randomCharacter = random.Next(0, 95);

        char randomAlphabet;

        return randomAlphabet = randomCharacter switch
        {
            0 => 'a',
            1 => 'b',
            2 => 'c',
            3 => 'd',
            4 => 'e',
            5 => 'f',
            6 => 'g',
            7 => 'h',
            8 => 'i',
            9 => 'j',
            10 => 'k',
            11 => 'l',
            12 => 'm',
            13 => 'n',
            14 => 'o',
            15 => 'p',
            16 => 'q',
            17 => 'r',
            18 => 's',
            19 => 't',
            20 => 'u',
            21 => 'v',
            22 => 'w',
            23 => 'x',
            24 => 'y',
            25 => 'z',
            26 => 'A',
            27 => 'B',
            28 => 'C',
            29 => 'D',
            30 => 'E',
            31 => 'F',
            32 => 'G',
            33 => 'H',
            34 => 'I',
            35 => 'J',
            36 => 'K',
            37 => 'L',
            38 => 'M',
            39 => 'N',
            40 => 'O',
            41 => 'P',
            42 => 'Q',
            43 => 'R',
            44 => 'S',
            45 => 'T',
            46 => 'U',
            47 => 'V',
            48 => 'W',
            49 => 'X',
            50 => 'Y',
            51 => 'Z',
            52 => '!',
            53 => '@',
            54 => '#',
            55 => '$',
            56 => '%',
            57 => '^',
            58 => '&',
            59 => '*',
            60 => '(',
            61 => ')',
            62 => '-',
            63 => '_',
            64 => '=',
            65 => '+',
            66 => '[',
            67 => ']',
            68 => '{',
            69 => '}',
            70 => ';',
            71 => ':',
            72 => '\'',
            73 => '"',
            74 => ',',
            75 => '.',
            76 => '<',
            77 => '>',
            78 => '/',
            79 => '?',
            80 => '\\',
            81 => '|',
            82 => '~',
            83 => '`',
            84 => 'ç',
            85 => 'ñ',
            86 => 'ß',
            87 => 'ø',
            88 => 'å',
            89 => 'æ',
            90 => 'œ',
            91 => 'ü',
            92 => 'ö',
            93 => 'ä',
            94 => '∞',
            _ => '\n',
        };
    }
}
