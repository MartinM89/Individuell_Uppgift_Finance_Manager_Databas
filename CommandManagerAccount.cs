using Individuell_Uppgift;

public class CommandManagerAccount
{
    public static UserAccount userAccount = new UserAccount();

    public static void Execute()
    {
        string userChoice = Console.ReadLine()!.ToUpper();

        switch (userChoice)
        {
            case "C"
            or "CREATE":
                userAccount.Create();
                break;

            case "L"
            or "LOGIN":
                userAccount.Login();
                CommandManagerTransaction.Execute();
                break;

            case "G"
            or "GUEST":
                // userAccount.GuestLogin();
                Console.Clear();
                Console.WriteLine("Guest Account");
                PressKeyToContinue.Execute();
                break;

            case "E"
            or "EXIT":
                Program.run = false;
                break;

            default:
                Console.WriteLine("Invalid Input.");
                break;
        }
    }
}
