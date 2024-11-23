public class CommandCenter
{
    public static void ExecuteLogin(string userChoice)
    {
        string[] command = userChoice.Split(" ");

        switch (command[0])
        {
            case "C"
            or "CREATE":

                break;

            default:
                break;
        }
    }
}
