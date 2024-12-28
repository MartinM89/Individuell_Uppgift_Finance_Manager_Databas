public class GetGuidForAdmin
{
    public static (Guid, string, bool) Execute(GetManagers managers)
    {
        Guid userGuid = Guid.Empty;
        string? targetUser = string.Empty;

        bool adminLoggedIn = managers.AccountManager.GetLoggedInUsername("Admin");

        if (adminLoggedIn)
        {
            Console.Clear();

            Console.CursorVisible = true;
            Console.Write("Username: ");
            targetUser = Console.ReadLine();
            Console.CursorVisible = false;

            if (string.IsNullOrEmpty(targetUser))
            {
                managers.UserMenuManager.ReturnToSameMenu();
                return (Guid.Empty, string.Empty, adminLoggedIn);
            }

            targetUser = targetUser[..1].ToUpper() + targetUser[1..].ToLower();

            userGuid = managers.AccountManager.GetUserGuid(targetUser);

            if (userGuid == Guid.Empty)
            {
                managers.UserMenuManager.ReturnToSameMenu();
                return (Guid.Empty, string.Empty, adminLoggedIn);
            }
        }

        return (userGuid, targetUser, adminLoggedIn);
    }
}
