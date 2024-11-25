using Individuell_Uppgift;

public class CommandManagerAccount
{
    public static UserAccount userAccount = new UserAccount();

    public static void Execute()
    {
        string userChoice = string.Empty;
        string hideUserChoice = HideCursor.Execute(userChoice).ToUpper();

        switch (hideUserChoice)
        {
            case "C":
                userAccount.Create();
                break;

            case "L":
                userAccount.Login();
                CommandManagerTransaction.Execute();
                break;

            case "G":
                // userAccount.GuestLogin();
                Console.Clear();
                Console.WriteLine("Guest Account");
                PressKeyToContinue.Execute();
                break;

            case "E":
                Program.run = false;
                break;

            default:
                Console.WriteLine("Invalid Input.");
                break;
        }
    }
}
