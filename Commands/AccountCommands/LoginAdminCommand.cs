using Individuell_Uppgift.Utilities;
using Npgsql;

public class LoginAdminCommand : Command
{
    public LoginAdminCommand(GetManagers getManagers)
        : base('Å', "Admin Login", getManagers) { }

    public override string GetDescription()
    {
        return "Hidden login (Admin login)";
    }

    public override void Execute()
    {
        Console.Clear();

        NpgsqlConnection connection = GetManagers.Connection;
        IMenuManager userMenuManager = GetManagers.UserMenuManager;

        string enteredPassword;

        Console.WriteLine("Admin Login Menu:\n");

        string username = "Admin";

        Console.Write("Enter password: ");
        enteredPassword = HidePassword.Execute();

        if (string.IsNullOrEmpty(enteredPassword))
        {
            userMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        bool isPasswordCorrect = PostgresAccountManager.CheckLoginDetailsIsCorrect(connection, username, enteredPassword);

        if (!isPasswordCorrect)
        {
            userMenuManager.SetMenu(new LoginMenu(GetManagers));
            return;
        }

        GetManagers.AccountManager.SetLoggedInUserId(username);

        Console.Clear();
        ChangeColor.TextColorGreen($"Login successful as admin.\n");
        PressKeyToContinue.Execute();

        userMenuManager.SetMenu(new TransactionMenuAdmin(GetManagers));
    }
}
